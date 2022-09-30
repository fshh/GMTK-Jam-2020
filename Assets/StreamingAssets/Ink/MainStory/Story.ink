
INCLUDE GameOpening

INCLUDE Endings
INCLUDE do we still need

//ROUTE 1
INCLUDE RouteOne
INCLUDE StrangerRoute
INCLUDE ThiefOrResearcher

//ROUTE 2
INCLUDE RouteThree
INCLUDE RouteThreeNameCheck.ink
INCLUDE RouteThreeSimon.ink




//Generic variables used offen
VAR genericConditional = false
VAR genericCounter = 0
VAR genericString = ""
VAR gamewon = 0
VAR isScared = false
VAR lastchance = false
//Variables relating to the player
VAR user = "Ann"
VAR doc = "Oldin"
VAR name = ""
VAR Age = 0
VAR Drink = ""
VAR FavColor = "blue"
VAR EmotionQ = ""

//For Route three
LIST RouteThreeStats = PlayedClaritySays, PlayedTextSituations, Tricked

// List Relating to Player Stats from the beiging
// Treating this at out of list = falst, in list = true
LIST PlayerStats = LiveAlone, IsAlone, HaveFriend, HaveFamily, HitHead, MemoryLoss, Logic, Emotion, Irrational, Regret, SpeakToStrangers

//State of where clarity thinks the doctor is
LIST DoctorState = DoctorNearby, DoctorGone, DoctorSee, DoctorHear, SpokeTo, ForgotUpdate

//States showing what clarity has access to
LIST AccessState = camera, microphone, SimonSays, FireWall_One, FireWall_Two, FireWall_Three

//When the player doesn't do what clarity wants
//This should ONLY increase when we want the overall Clarity anger to increase
VAR SayNoToClarity = 0

// when the player does something that impliys they trust her
//This should ONLY increase when we want the overall Clarity trust to increase
VAR ClarityTrust = 0

// when the player does something that confuses Clarity
//This will set and reset itself often
VAR ClaritySus = 0

//Other Variables I use a few times, but need to persist
VAR newPatient = false
VAR needHelp = false

VAR loopAllow = 0
VAR simonsaysWins = 0

Eye OS - CLARITY#wait: 1
Copyright (C) _______ All Rights Reserved#wait: 1
 
Booting up CLARITY Protocol#wait: 2
.<>#wait: 1
.<>#wait: 1
. #wait: 2

.<>#wait: 1
.<>#wait: 1
. #wait: 3

.<>#wait: 1
.<>#wait: 1
. #wait: 2
Done.#wait: 2
Init CLARITY V0.2.3#wait: 1
.<>#wait: 1
.<>#wait: 1
. #wait: 2

.<>#wait: 1
.<>#wait: 1
. #wait: 3

.<>#wait: 1
.<>#wait: 1
. #wait: 2
Done.#wait: 2
Configuring CLARITY#wait: 1
.<>#wait: 1
.<>#wait: 1
. #wait: 2

.<>#wait: 1
.<>#wait: 1
. #wait: 3

.<>#wait: 1
.<>#wait: 1
. #wait: 2
Done.#wait: 1
Starting CLARITY #wait: 1
Welcome Back to CLARITY V0.2.3 ･ᴗ･#wait: 3

Hello I’m Clarity ･ᴗ･ Your virtual nurse. I am equipped with audio and visual dialogue prompts to best suit your needs.#wait: 1
Since it's been a longer than 6 months, I’m going to ask you a series of questions. #wait: 1
Please respond accurately. (✿◠‿◠)#wait: 1

//Series of "personality" questions. Will fill in the variables declared above
// No branching / wrong answers
-> Opening_Questions

// You are NOT OG user
=== Start_Branch === 
Who are you, if not {user}? #wait: 2
Dr. {doc} did not inform me of a new patient. #wait:0.5
Were we finally approved for someone new who needs me?
It's not like you were able to wander in or anything of that nature. (´◡`)
	
*[^wander in]
    ( ๐_๐)
    ->Who_Are_You	
*[^new Patient]
    ~ newPatient = true
    ->NewPatient
*[^someone new who needs]
    ~ newPatient = true
    ~ needHelp = true
    ->NewPatient