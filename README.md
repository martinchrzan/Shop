# Shop
Example of Web API, Client dll + client app to handle request

To use Client app, you need to update server address ->
start Shop project, once the project is started in a local host, note the address in your browser.
It should look like: http://localhost:someNumber 

Copy that and update it in: ServerConnectionResolver.GetServerApiBaseAddress() method (ClientApp project)
so it will look like:
http://localhost:someNumber/api 