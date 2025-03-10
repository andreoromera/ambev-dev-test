# Developer Evaluation Project

## Greetings
Hi dear evaluator!<br />
Thank you for taking the time to evaluate this test!
I hope this is enough to show my skills as a senior developer.<br />

## How to run the suite
```
1. Clone this git repository. Code is in main branch.
2. Open up a powershell terminal and navigate to the root folder of the project
3. Run the script .\start.ps1
4. Wait until the script finishes (it can take a while in order to download and install images)
5. Once the script finishes, you should have the website up and running at http://localhost:3000
6. Powershell will open a browser window automatically for you
```

You don't need to worry about the database creation and data seeding, I have it cover for you ;-)<br />
I have implemented automatic migration (including seeding data) to create and populate the necessary tables.<br />

## Default Employee
I have created a default employee with the following credentials:<br />
```
Email: john@ambev.com.br
Password: P@szW0rd
Role: Admin
```

You can log in with this user to be able to create as many employees as you want :-).<br />

## Testing the webapi
Once you have the webapi up and running, you can inspect swagger (https://localhost:7199/swagger/index.html) to interact with the endpoints.<br>
If you prefer, you can find at the root folder a Postman Collection file you can import in Postman to test the api.

Either way, you should:
```
1. Call any endpoint without authentication to see error 401 (Unauthorized)
2. Call the signin endpoint to get a valid token. There is a script already attached to this action 
   that creates a Postman Collection Variable with the returned token, so you don't have to manually 
   set the bearer token for each endpoint yourself :-)
3. Now you can expect to have successful calls to whatever endpoint you like.
```