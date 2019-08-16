# EntrySample

# How it Works:

1) First of all open a cmd and go to the project`s folder.

2) start the rabbit client writing the command `docker-compose up`

3) start the project.

Currently i`m not using any database.

there is 2 accounts: 
<p> 1)
<p> Branch: 1
<p> Number: 2
<p> Digit: 1
<p> Balance: 1000

<p> 2)
<p> Branch: 1
<p> Number: 3
<p> Digit: 1
<p> Balance: 0


<p> It's possible to check the message of the current execution accessing the rabbit on 127.0.0.1:15672
with the user and password `guest`

<p> altough you'll need to bind the exchange Transfers to a queue.
there is 

3 routing keys

        error
        success
        insufficient-funds
