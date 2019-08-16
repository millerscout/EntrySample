# EntrySample

# How it Works:

1) First of all open a cmd and go to the project`s folder.

2) start the rabbit client writing the command `docker-compose up`

3) start the project.


Currently i`m not using any database.

there is 2 accounts: 
1)
Branch: 1
Number: 2
Digit: 1
Balance: 1000

2)
Branch: 1
Number: 3
Digit: 1
Balance: 0


It's possible to check the message of the current execution accessing the rabbit on 127.0.0.1:15672
with the user and password `guest`

altough you'll need to bind the exchange Transfers to a queue.
there is 
3 routing keys

        error
        success
        insufficient-funds
