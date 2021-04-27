# Fauna database streaming demo application

This is a sample C# application that demostrates how we can use fauna driver to listen to updates events of a document in a database.

## Setup a database for this project

This project assumes that you're going to run it against a fauna database, so you'll need to create one in a fauna dashboard,  
as well as a collection and a document.  

I have created a collection "Categories" and a document iside it which got an id "287626591762121219".
Change the collection name and a document id at this line to yours:  
https://github.com/parkhomenko/fauna-csharp-streaming-app/blob/main/fauna-csharp-streaming-app/Program.cs#L28

When you have your database created you need to generate a secret for it to be able to connect from application,  
after that, you need to setup 2 environment variables:
- FAUNA_DOMAIN
- FAUNA_ROOT_KEY
You can add this variable in the "Run configuration" of your IDE or from terminal:
```
export FAUNA_DOMAIN=db.fauna.com
export FAUNA_ROOT_KEY=my-secret-key
``` 

## Run the application

A repo contains a C# solution (with .NET 5.0 target framework) that you can open in your favourite IDE.
You can also just switch to a project directory in your terminal and run:
```
cd ./fauna-csharp-streaming-app
dotnet run
```

If you set up everything correctly you should see a "start" event in the terminal:
```
ObjectV(type: StringV(start),txn: LongV(1619548421450000),event: LongV(1619548421450000))
```
After that, application should wait for 3 more events before it exits,  
in order to see new events, try to update the document you defined as described above:  
you can add a new record to this document or update an existing one.  
You should see events similar to this one:
```
ObjectV(type: StringV(version),txn: LongV(1619548443570000),event: ObjectV(action: StringV(update),document: ObjectV(ref: RefV(id = "287626591762121219", collection = RefV(id = "Categories", collection = RefV(id = "collections"))),ts: LongV(1619548443570000),data: ObjectV(name: StringV(Phones),quantity: LongV(126),size: StringV(test),another: StringV(one),more: StringV(fields),afield: LongV(181),phone: StringV(+3809711111116)))))
```
