db.createUser(
    {
        user: "adminadmin",
        pwd: "adminadmin",
        roles: [
            {
                role: "readWrite",
                db: "tmp"
            }
        ]
    }
);