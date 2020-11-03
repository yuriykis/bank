// JavaScript source code
conn = new Mongo();
db = conn.getDB("Bank");

db.Accounts.drop()
db.Users.drop()
db.Transactions.drop()

db.Accounts.insertMany([
	{ amount: 5000, account_number: 1  },
	{ amount: 1000, account_number: 2  },
]);

var account_1 = db.Accounts.findOne({ account_number: 1 });
var account_2 = db.Accounts.findOne({ account_number: 2 });

db.Users.insertMany([
	{ name: 'user_1', password: 'password', account_id: account_1._id},
	{ name: 'user_2', password: 'password', account_id: account_2._id},
])

var user_1 = db.Accounts.findOne({ amount: 5000 });
var user_2 = db.Accounts.findOne({ amount: 1000 });

db.Transactions.insertOne(
	{ sender_account_id: user_1._id, reciver_account_id: user_2._id, amount: 500 }
)