# PhoneDirectory

- PhoneDirectory project has 5 main layers
- Postman collection v2.1 is available as PhoneDirectory.postman_collection.json. To access all end points kindly import the json file
- All migration files is inside of Repository Layer

- All API's have a common response type called Response

# Response
    {"Status":200,
    "isSuccess": true,
    "message": "",
    "data": {},
    "dataList":[]
    }

# Relations
- Person and Information entities were created with one-to-many relation
- Both Person and Information entity can be deleted with soft delete (Makes their status 0)
- A person can have **many informations**
- Windows Sql Server is used as database

- %60 unit testing code coverage is not available in the project because of the time limitation and ongoing heavy work schedule

# Before Run

-Run following command to launch redis as a container
```bash
 docker run -d -p 6379:6379 --name redis redis
```
And finally, Use the package manager console to execute the migration after selecting PhoneDirectory.Repository as a startup project

```bash
update-database
