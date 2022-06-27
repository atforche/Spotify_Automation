# Spotify Automation

### Objectives
1. Create a nightly process that will scan each of my playlists, determine if any songs have been added or removed, and then make the change to my master playlist (where all of my songs are compiled into a single playlise)
2. Add a notification mechanism to inform me of any changes that were processed during the nightly run
3. Create an automated testing harness that will populate a test user with data, make modifications, and then run the process to determine that things are working as expected
4. Create an automated GitHub build workflow that compile the program and run automated tests to determine if newly pushed code still works correctly. Additionally, if the build and tests succeed, the workflow will deploy the new version of the program to my web server.

### Side Goals
1. Create a program that can migrate all the songs in my Apple Music library over into Spotify

### To-Do
1. Decide on a programming language
2. Get connected to the Spotify API
3. Create a test user and populate the data for testing
4. Start developing