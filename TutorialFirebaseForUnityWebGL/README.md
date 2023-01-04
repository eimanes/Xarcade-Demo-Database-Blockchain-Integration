# Tutorial Firebase for Unity WebGL using REST API
Integrate firebase with Unity WebGL using REST API
Firebase Database in Unity with REST API

## Demo for Xarcade Games:


## Why use REST?
When it comes to Unity, Firebase offers a complete SDK in order to easily integrate its different services (database, authentication, functions…).
So why should we use REST APIs instead?

The main reason is that the Firebase SDK is not available for Standalone Unity Builds (Windows, MacOS, Linux). There is a desktop workflow, but as it is stated on the Firebase documentation:

Caution: Firebase Unity SDK desktop support is a beta feature. This feature is intended only for workflows during the development of your game, not for publicly shipping code.

Moreover, I have encountered a bug that simply makes Firebase SDK not work on Unity 2019.3. This is not a surprise because, at the time I am writing this, that version of Unity is still in Alpha stages, but it is something to consider.

These are the functionalities I plan on implementing for this tutorial
- Ability to post a user to the database
- Ability to retrieve a user from the database given its id
- Ability to retrieve all users from the database (and all their ids)

## What we’ll be using
First things first, we’ll be using two external libraries that will help us in our tasks:
- Rest Client which is available on the asset store
- Full Serializer which you can find here

## The User object
Let’s create a User object responsible for holding data that we’ll later upload to the Firebase Database.
```
using System;

/// <summary>
/// The user class, which gets uploaded to the Firebase Database
/// </summary>

[Serializable] // This makes the class able to be serialized into a JSON
public class User
{
    public string name;
    public string surname;
    public int age;

    public User(string name, string surname, int age)
    {
        this.name = name;
        this.surname = surname;
        this.age = age;
    }
}
```


## Connect to Firebase
If you haven’t already, create a new Firebase project!

Once you are in your project, click on Database in the toolbar and create a Realtime Database with security rules in testing mode.

Once that is done, go to your Project Settings and copy your Project Id, we’ll need that in just a moment. For this example, my Project Id will be nico_the_weather, for no logical reason.

## The DatabaseHandler Class
Let’s take this slowly. First of all, we’ll need a reference to the database REST Endpoint we’ll send the requests to:

```
private const string projectId = "nico-the-weather"; // You can find this in your Firebase project settings
private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
```

## The PostUser Function
Now it’s time to write our PostUser function that will upload a new user onto our database. The function will take as a parameter the user that will be uploaded and also a userId that will identify the user so that we can retrieve it afterward from the database. We’ll be doing a Put request using the RestClient Asset linked above.

```

    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    public static void PostUser(User user, string userId)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json", user);
    }
 ```

That means that, for instance, if the userId is 10, the user will be uploaded on Firebase Database and it will be available at this endpoint: https://nico_the_weather.firebaseio.com/users/10.json

The user will look something like this on the Firebase Database:
![image](https://user-images.githubusercontent.com/80232250/210306376-0313d818-8a46-4bb7-a2c5-d1cb49b46d47.png)


Depending on your connection speed, the request may take some time. It is good practice to know exactly when the request is complete. Luckily for us, RestClient has a simple way to solve this very issue (with .Then()):
```
    public static void PostUser(User user, string userId, PostUserCallback callback)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json", user).Then(response => { 
            Debug.Log("The user was successfully uploaded to the database");
        });
    }
```

Line 4 of that snippet of code will only execute once the request has been successfully fulfilled. We’ll include a delegate as a parameter of our function so that whoever calls the PostUser function can decide what to do after the request is complete, like this:
```
    public delegate void PostUserCallback();
    
    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    public static void PostUser(User user, string userId, PostUserCallback callback)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json", user).Then(response => {
            callback();
        });
    }
```

## The GetUser Function
Let’s now do the exact opposite: a Get request to retrieve a user with a specific userId:
```

    public delegate void GetUserCallback(User user);
    /// <summary>
    /// Retrieves a user from the Firebase Database, given their id
    /// </summary>
    /// <param name="userId"> Id of the user that we are looking for </param>
    /// <param name="callback"> What to do after the user is downloaded successfully </param>
    public static void GetUser(string userId, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user =>
        {
            callback(user);
        });
    }
```

The .Then() will now have our already deserialized object User as a parameter; we can use that as a parameter in our function delegate.

## The GetUsers Function
We’ll now create a function that will get all of the users in our database! Boom!

Before doing that, let’s think about how data in Firebase Realtime Database is structured. Let’s look at this list of users in JSON:

```
{
    "users": {
        "10": {
            "age": 55, 
            "name": "Christopher", 
            "surname": "Eccleston"
        }, 
        "11": {
            "age": 36, 
            "name": "Matt", 
            "surname": "Smith"
        }, 
        "12": {
            "age": 61, 
            "name": "Peter", 
            "surname": "Capaldi"
        }
    }
}
```

As you can see, for each user, we have a Key (the userId) and a Value (the user age, name, and surname).

At example, for the second user in the list, the Key is [11] and the Value is [“age”: 36, “name”: “Matt”, “surname”: “Smith”].

In C#, that JSON is mapped into a Dictionary<string, User>, which is a list that pairs up a userId (Key) with a User (Value).

Unfortunately, Unity’s internal serializer is not good enough to serialize such a complex object; that is why we are forced to use an external library, FullSerializer, which is linked above. With that in mind, let’s write the function that will retrieve all of our users:
```
    private static fsSerializer serializer = new fsSerializer();
    public delegate void GetUsersCallback(Dictionary<string, User> users);
    /// <summary>
    /// Gets all users from the Firebase Database
    /// </summary>
    /// <param name="callback"> What to do after all users are downloaded successfully </param>
    public static void GetUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}users.json").Then(response =>
        {
            var responseJson = response.Text;

            // Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
            // to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, User>), ref deserialized);

            var users = deserialized as Dictionary<string, User>;
            callback(users);
        });
    }
 ```

First and for most, the URL of the request no longer needs a userId, since we are downloading the entire users branch. Secondly, we are no longer internally deserializing the result (note that the result is not a <User> anymore but a response object), unlike the other functions. This means, the response.Text parameter we will retrieve from the request will be some JSON.

Then, we deserialize that JSON into the type we want (we said above it had to be a Dictionary<string, User>) and we put that as a parameter of our delegate function.

If you have questions on the FullSerializer syntax, check out their documentation on GitHub.

So, in conclusion, this is how our DatabaseHandler class is looking:
  
using System.Collections.Generic;
using FullSerializer;
using Proyecto26;
 
public static class DatabaseHandler
{
    private const string projectId = "nico-the-weather"; // You can find this in your Firebase project settings
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";
    
    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(User user);
    public delegate void GetUsersCallback(Dictionary<string, User> users);
    
    
    /// <summary>
    /// Adds a user to the Firebase Database
    /// </summary>
    /// <param name="user"> User object that will be uploaded </param>
    /// <param name="userId"> Id of the user that will be uploaded </param>
    /// <param name="callback"> What to do after the user is uploaded successfully </param>
    public static void PostUser(User user, string userId, PostUserCallback callback)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json", user).Then(response => { callback(); });
    }

    /// <summary>
    /// Retrieves a user from the Firebase Database, given their id
    /// </summary>
    /// <param name="userId"> Id of the user that we are looking for </param>
    /// <param name="callback"> What to do after the user is downloaded successfully </param>
    public static void GetUser(string userId, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); });
    }

    /// <summary>
    /// Gets all users from the Firebase Database
    /// </summary>
    /// <param name="callback"> What to do after all users are downloaded successfully </param>
    public static void GetUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}users.json").Then(response =>
        {
            var responseJson = response.Text;

            /* Using the FullSerializer library: https://github.com/jacobdufault/fullserializer
             to serialize more complex types (a Dictionary, in this case)
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, User>), ref deserialized);

            var users = deserialized as Dictionary<string, User>;
            callback(users);
        });
    }
}


## The Main Class
We’ve written all the logic involved to make our requests do their job! Now it’s time to call those functions we created and see if our work pays off:
```

using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        
    }
}
```

Thanks to Unity magic, that function right there will be executed right when we start our application; so let’s now write some requests and run the app to test them!


This code should now create a new User and then print the information about the user with an id of 11. In fact, if we run the app and check our logs, this is what we see:
```
using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        var user2 = new User("Peter", "Capaldi", 61);
        DatabaseHandler.PostUser(user2, "12", () =>
        {
            DatabaseHandler.GetUser("11", user =>
            {
                Debug.Log($"{user.name} {user.surname} {user.age}");
            });
        });
    }
}
```
![image](https://user-images.githubusercontent.com/80232250/210306977-29dfe183-96c0-473a-9df3-ae8e145c2788.png)

For our last test, let’s replace the code above with this one:
```
using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        DatabaseHandler.GetUsers(users =>
        {
            foreach (var user in users)
            {
                Debug.Log($"{user.Value.name} {user.Value.surname} {user.Value.age}");
            }
        });
    }
}
```


This should retrieve all users in our database…
![image](https://user-images.githubusercontent.com/80232250/210307056-711747d2-d735-443e-99c4-cf0e4ffcce41.png)

And that’s it!
