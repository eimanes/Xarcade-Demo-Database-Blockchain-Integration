# Xarcade-Demo-Database-Blockchain-Integration

# Database Integration for WebGL Games

## Demo WebGL Build in Xarcade
[Demo Token Test](https://xarcade-gamer.proximaxtest.com/game-details/B02995559FA34CEC)

## Internal Database (PlayerPrefs)
-	PlayerPrefs is the best class to use to keep the data in the games.
-	Unity stores up to 1MB of PlayerPrefs data using the browser’s IndexedDB API.
-	IndexedDB is a low-level API for client-side storage of significant amounts of structured data, including files/blobs. This API uses indexes to enable high performance searches of this data.

## External Database (Firebase)
-	The simplest way is using rest api to communicate between webgl games with firebase
-	Reasons? Whenever users refresh the webgl game page, the data will lose its data. So the game needs to have its own external database.
-	We recommend you use Firebase as the database will save in real-time.
-	We use REST because Firebase SDK is not available for Standalone Unity Builds (Windows, MacOS, Linux).
-	Developers are recommended to look forward in the documentation on Firebase Database in Unity with REST API.

### Flowchart



## More Tutorial
[Tutorial Unity WebGL with Firebase using REST API](https://github.com/eimanes/Xarcade-Demo-Database-Blockchain-Integration/tree/main/TutorialFirebaseForUnityWebGL)
