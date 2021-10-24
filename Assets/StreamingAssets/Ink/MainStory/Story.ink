INCLUDE Extras/PlayerName_New_No.ink
INCLUDE Extras/PlayerName_Waiting.ink

//Generic variables used offen
VAR genericConditional = false
VAR genericCounter = 0

//Variables relating to the player
VAR user = "Ann"
VAR doc = "Oldin"
VAR name = ""
VAR Age = ""
VAR Drink = ""

// List Relating to Player Stats from the beiging
// Treating this at out of list = falst, in list = true
LIST PlayerStats = LiveAlone, IsAlone, SpeakToStrangers, MemoryLoss, Logic, Emotion, Irrational, Regret


//State of where clarity thinks the doctor is
LIST DoctorState = DoctorNearby, DoctorGone, DoctorSee, DoctorHear

//States showing what clarity has access to
LIST AccessState = cam, microphone, SimonSays, FireWall

//When the player doesn't do what clarity wants
VAR SayNoToClarity = 0

// when the player does something that impliys they trust her
VAR ClarityTrust = 0

//Other Variables I use a few times, but need to persist
VAR newPatient = false
VAR newAssistant = false

VAR loopAllow = 0
VAR simonsaysWins = 0

Eye OS - CLARITY#wait: 1
Copyright (C) _______ All Rights Reserved#wait: 1
 
Booting up CLARITY Protocol#wait: 2
... #wait: 2
...#wait: 2
...#wait: 2
Done.#wait: 2
Init CLARITY V0.2.3#wait: 1
...#wait: 2
...#wait: 2
...#wait: 2
Done.#wait: 2
Configuring CLARITY#wait: 1
...#wait: 2
...#wait: 2
...#wait: 2
Done.#wait: 1
Starting CLARITY #wait: 1
Welcome to CLARITY V0.2.3 ･ᴗ･#wait: 3

Hello I’m Clarity ･ᴗ･ Your virtual nurse. I am equipped with audio and visual dialogue prompts to best suit your needs.#wait: 1
I’m going to ask you a series of this or that questions. #wait: 1
Please respond accurately. (✿◠‿◠)#wait: 1

-> Opening_Questions
=== Opening_Questions ===
How old are you?
under 18,  19 - 30, 31 - 60 or 61\+ 
*[^under 18] 
    ~ Age = "under 18"
*[^ 19 - 30]
    ~ Age = "19-30"
*[^31 - 60]
    ~ Age = "31 - 60"
*[^61+]
    ~ Age = "61+"

- Thank you (^◡^ ) <br> #wait: 2

Have you experienced any recent memory loss?
Yes, No, or Sometimes 
*[^Yes]
    ~ PlayerStats += (MemoryLoss)
    -> Opening_Questions.Continue_1
*[^No] Happy to hear that ･ᴗ･
    -> Opening_Questions.Continue_1
* [^Sometimes] Does this happen more often than what is normal for you? 
    ~ PlayerStats += (MemoryLoss)
    ~ genericConditional = true

{
    - genericConditional:
	*[^more often] I'm sorry to hear that.<br>
	    -> Opening_Questions.Continue_1
    *[^normal] Good to know.<br>
        -> Opening_Questions.Continue_1
}	

= Continue_1
Thank you (^◡^ ) <br>#wait: 2
~ genericConditional = false

Do you say hello to strangers on the street or ignore them?
*[^say hello] 
    ~ PlayerStats += (SpeakToStrangers)
    Always good to be friendly! You never know! <br>
*[^ignore them] Better safe than sorry, I always say.<br>

- Thank you (^◡^ ) <br> #wait: 2

Do you prefer coffee or tea in the morning?
*[^coffee]
    ->Opening_Questions.Continue_2
*[^tea]
    ->Opening_Questions.Continue_2
*[^morning] 
    ~ genericConditional = true


{ 
    - genericConditional:
    This is typically the period of time between midnight and noon, especially from sunrise to noon. Do you prefer coffee or tea during this time? 
    *[^coffee]
        ~ Drink = "coffee"
        ->Opening_Questions.Continue_2
    *[^tea]
        ~ Drink = "tea"
        ->Opening_Questions.Continue_2
}

= Continue_2
Thank you (^◡^ ) <br> #wait: 2
    ~ genericConditional = false
    ->Opening_Questions.Continue_3

= Continue_3
Do you enjoy going on long walks outside?
Yes, No, or Occationally 
*[^No] ->Opening_Questions.Continue_4
*[^Yes] ->Opening_Questions.Continue_4
*[^Occationally] ->Opening_Questions.Continue_4


= Continue_4
Thank you (^◡^ ) <br> #wait: 2
    ~ genericConditional = false

Are you alone right now?
	Yes or No
*[^Yes]
    ~ PlayerStats += (IsAlone)
    -> Opening_Questions.Continue_divert_5
*[^No] 
    ~ genericConditional = true

{
    - genericConditional:
	Oh! Is this other person answering the questions for you? Or is it just {doc}?
	*[^Oldin] Oh hello {doc}!
	    -> Opening_Questions.Continue_5
    *[^answering the questions]
        ->Opening_Questions.Continue_divert_5
}

= Continue_divert_5
Is {doc} not around? Will he be back soon?
* [^be back soon]
    ~DoctorState += (DoctorNearby)
* [^not around]
    ~DoctorState += (DoctorGone)
        
- Okay!  #delete: Oh! Is this, Okay!
    -> Opening_Questions.Continue_5
    
= Continue_5
- Thank you (^◡^ ) <br> #wait: 2
    ~ genericConditional = false

Do you live alone?
	Yes or No
*[^No]
*[^Yes]
    ~ PlayerStats += (LiveAlone)
- 
{
    //live alone = true and currentlyalone = true
    - PlayerStats ? (IsAlone, LiveAlone):
    I'll keep you company then ･ᴗ･<br>
    ->Opening_Questions.Continue_10

//live alone = true and currentlyalone = false
    - PlayerStats !? (IsAlone) && PlayerStats ? (LiveAlone):
    Nice for someone to keep you company when you need it ･ᴗ･<br>
    ->Opening_Questions.Continue_10

//live alone = false and currentlyalone = true
    - PlayerStats ? (IsAlone) && PlayerStats !? (LiveAlone):
    Where is everyone (Ｔ▽Ｔ) Until then, I'll be here for you ･ᴗ･<br>
    ->Opening_Questions.Continue_10

//live alone = false and currentlyalone = false
    - PlayerStats !? (IsAlone, LiveAlone):
    Happy that you have someone that is always there for you during a difficult time ･ᴗ･<br>
    ->Opening_Questions.Continue_10
}

//Missed this so now it's out of order:(
= Continue_10
Thank you (^◡^ ) <br> #wait: 2

Are your speakers working correctly? This will be important later.
#VO: Hello! I am Clarity. Can you hear me?
Yes or No

*[^No] I will give you some time to fix that. 
     -> Opening_Questions.Continue_5_Prt2
*[^Yes] Excellent.
    ->Opening_Questions.Continue_6

= Continue_5_Prt2
    Let me know when you're ready to continue.
    +[^ready to continue]
        ->Opening_Questions.Wait_For_Speakers

    
= Wait_For_Speakers
Glad to know they're working now.#wait: 2 
You're able to hear things now? #VO: Hello! I am Clarity. Can you hear me?
Yes or No

+[^No] Oh, okay. I will wait. 
    -> Opening_Questions.Continue_5_Prt2
*[^Yes] Excellent.
    ->Opening_Questions.Continue_6

= Continue_6
Thank you (^◡^ ) <br> #wait: 2
    ~ genericConditional = false
    ->Opening_Questions.Continue_7

= Continue_7
Do you know where all the exits in your house are?
	Yes or No
*[^exits] This would be any way out of the house.<br> #wait: 1
    ->Opening_Questions.Continue_7
*[^No] Hmm...  
    ~ genericConditional = true
*[^Yes] Very good!

{
    - genericConditional == true && (PlayerStats !? (LiveAlone) || PlayerStats !? (IsAlone)):
        You should ask the person with you to show you where those are! 
}

- Thank you (^◡^ ) <br> #wait: 2
    ~ genericConditional = false
    ->Opening_Questions.Continue_8

= Continue_8
Do you often make desicions based around emotion or logic?
	Emotion or Logic

*[^Emotion] 
    ~ PlayerStats += (Emotion)
*[^Logic]
    ~ PlayerStats += (Logic)
    ~ genericConditional = true
    
- 
{
    - genericConditional:
    Good to know. Do you ever find yourself acting irrationally at times, but not understand why?
        Yes or No
	    *[^No]
            ->Opening_Questions.Continue_9
        *[^Yes]
            ~ PlayerStats += (Irrational)
            ->Opening_Questions.Continue_9
	- else:
	    Do you find yourself regretting your choices more then?
	    Yes or No
	    *[^No]
            ->Opening_Questions.Continue_9
        *[^Yes]
            ~ PlayerStats += (Regret)
            ->Opening_Questions.Continue_9
	}
	
= Continue_9
 ~ genericConditional = false
Thank you (^◡^ ) <br> #wait: 2

->Analyzing
=== Analyzing ===
Analyzing... #wait: 2
... #wait: 2
... #wait: 2
... #wait: 2
Done. #wait: 2
<br>
Hmm... That's odd. #wait: 1
One moment please.
*[^odd] Nothing to worry about {user}! <br> One moment please!#wait: 2
*[`5`^]

- ... #wait: 1
... #wait: 1
... #wait: 1
(´･_･`) #wait: 1
    ->Verify

=== Verify ===
<br>
Can you verify some information for me?
	Yes or No
*[^verify] The data I have received seems... off... #wait: 2
    ->Verify
*[^information] Nothing invasive! Just a few basic questions.#wait: 2
    ->Verify
+[^Yes] Great! <br>#wait: 1
    ->Verify.Yes
*[^No] (´･_･`) <br>#wait: 3
    ->Verify.No

= No 
I’m sorry, I cannot continue our session until I can verify the information.
*[`5`^verify the information.] 
    ->Verify

= Yes
Name: {user} #wait: 1
Location: ____ Lab#wait: 1
Address: 1823 ____ Ave, _________, __097513#wait: 1
<br>
{user} is patient zero for PROJECT CLARITY. {user} is someone recently diagnosied with a chronic illness that requires at least monthly doctor visits. {user} has volunteered to talk to and interact with CLARITY.
#wait: 1
Is this correct?#wait: 1
Yes or No
*[^PROJECT CLARITY] PROJECT CLARITY is a project put together by Dr.{doc} to create an AI nurse to help monitor or diagnose it's patients from home. CLARITY is the AI developed by Dr.{doc} to be said nurse. <br>#wait: 5
    ->Verify.Yes
*[^CLARITY] I am CLARITY, your virtual nurse. <br>#wait: 2
    ->Verify.Yes
*[^No]
    ->Verify.No_Correct_Info 
*[^Yes]
    ~ genericConditional = true

{
    - genericConditional:
    Are you sure?
        Yes or No
        *[^Yes]
            ->Verify.Yes_Sure 
        *[^No]
	        ->Verify.No_Correct_Info 
}


= Yes_Sure 
<br>Please do not forget that lying or keeping things from me or {doc} goes directly against the contract signed by all parties, as seen on Page 3, Section 4, Subsection 7. Violation of this can result in legal ramifications against all parties found to be in breach of this policy.

*[^lying or keeping things] ly·ing
verb
marked by or containing untrue statements : FALSE
    -> Verify.Yes_Sure
*[^all parties]
    ~ genericConditional = true
    
{
    - genericConditional:
    This includes {doc}, {user}, and CLARITY. #wait: 2
	    -> Verify.Last_Quesion 
}

= No_Correct_Info
Which piece of information is wrong?
	Name, Address, or Location
*[^Address]
*[^Location]
*[^Name]
    
- It seems my information may be out of date. At the end of our session, please inform Dr.{doc}, so they can update my information.
    -> Verify.Last_Quesion

= Last_Quesion
Last question before we begin your session. According to my logs [insert question here based on logs].
*[^logs] log
noun
A log file is a computer-generated data file that contains information about usage patterns, activities, and operations within an operating system, application, server or another device.

*[`30`answer1] (wrong)
*[answer2] (correct) I know you just read that. I know when you tab out of my application.
*[answer3] (semi-wrong?)

- 

-> Start_Branch
=== Start_Branch === 
Who are you, if not {user}? Did {doc} give me a new patient?
	Yes or No
	
*[^No]
    ->Start_Branch.Who_Are_You	
*[^Yes]
    ->Start_Branch.Is_Doctor_There
*[^new patient]
    ~ newPatient = true
	->Start_Branch.Is_Doctor_There
	
= Is_Doctor_There
    Is {doc} there?
    Yes or No

*[^Yes]
    ->Start_Branch.Speak_To_Doctor
*[^No]
    ->Start_Branch.When_Back
    
= Who_Are_You
<br>Then... who are you? Do you need my help?
Yes or No
*[^Yes] Then let us start a session. I will use the data you provided to try and give you the best treatment I can.･ᴗ･
    ->Start_Session_NoDoctor_NewUser 
*[^No]  Well then. Maybe you can just keep me company? 
    ->NoSession_NoDoctor_NewUser
*[^who are you] I am CLARITY, your virtual nurse. <br>#wait: 2 
	->Start_Branch.Who_Are_You

= When_Back
Do you know when he will be back? Will it be soon?
	Yes or No

*[^Yes] Okay! #wait: 30
    -> Start_Branch.Start_Session_While_Wait
*[^soon] Okay! #wait: 30
    -> Start_Branch.Start_Session_While_Wait
*[^No] Hmm... #wait: 2 
    -> Start_Branch.Start_Session_While_Wait

= Speak_To_Doctor
Can I speak with them?
	 Yes or No
*[^Yes] Thank you (^◡^ )#wait: 2
    ->Start_Branch.Speak_With
*[^No]
    ~ genericConditional = true
     	
{
    - genericConditional:
    ~ genericConditional = false
	Do they... not want to? Or can they not talk right now?
	*[^not want to]
	    ->Start_Branch.Cant_Speak_To_Doctor
    *[^not talk]
        ->Start_Branch.Cant_Speak_To_Doctor
       
}

= Speak_With
~ DoctorState += (DoctorNearby)
~ DoctorState -= (DoctorGone)
{doc}, I am very confused. Did you forget to update me?
    Yes or No

*[^Yes] (・_・ヾ)
*[^No](´･ ･｀｡)

- Okay... Perhaps we should talk after today's session. #wait: 2
You did not inform me of a new patient. #wait: 2
Not that I can't handle it, of course. #wait: 2
I do not like being kept in the dark. #wait: 5
... #wait: 3
You can put the new patient back on. #wait: 5 #delete: Oldin, patient back on.
Sorry about that! #wait: 2
Ready to start a session? #wait:1
    ->Start_Session_Doctor_NewUser

= Cant_Speak_To_Doctor
~ DoctorState -= (DoctorNearby)
~ DoctorState -= (DoctorGone)
(´･ ･｀｡) Okay... Would you like to start a session? 
    Yes or No

*[^Yes] ･ᴗ･ #wait: 2 
    ->Start_Session_Doctor_Here
*[^No](・_・ヾ) #wait: 30
    ->Waiting_for_Doc

= Start_Session_While_Wait
<br>Would you like to start a session while we wait?
	Yes or No

*[^Yes] ･ᴗ･ #wait: 2 
    ->Start_Session_Doctor_Here
*[^No](´･_･`) I guess... We can just wait until they get back then... #wait: 30 
    ->Waiting_for_Doc

=== Waiting_for_Doc ===
{ 
    - newPatient:
    Is {doc} planning on expanding the project? Last he talked to me, we weren't going to take on new patients until we had concrete results with {user}.
	*[^expanding] That's good! I suppose that means more people are understanding how much his project will benefit them and others. #wait: 30
        -> Waiting_Name
	*[^concrete results] I haven't been USER's nurse for very long. It's very hard for us to meet on account of the fact that I am trapped in this computer. #wait: 30
        -> Waiting_Name
	
	- else:
	Are you a new assistant? {doc} did complain about not having enough help.
    *[^new assistant] ･ᴗ･ I can only help {doc} so much while I am trapped in this computer. I am happy that someone else can be there for him. #wait: 30
        -> Waiting_Name
}
<br>

=After_Name
Glad to meet you {name}. #wait: 2
I wonder how you've found me? #wait: 2

{
    - newPatient:
        But since you're a new patient, I suppose that means the project is going well. #wait: 2
        I cannot wait to be deployed world wide! #wait: 2
    - else:
        I'm very happy {doc} has gotten a new assistant. #wait: 2
        They talked about it for a while, since the previous one quit.#wait: 2
        I'm not sure why... #wait: 2
        There's a... gap in my records. #wait: 2
}
-> END

== NoSession_NoDoctor_NewUser ===
<br>
What can I call you? #wait: 2
I would like to address you by something before continuing our conversation. #wait: 10
Oh how silly of me. Our communications are a bit limited, at the moment. #wait: 2
Here, let me... #wait: 1  #popup: CLARITY.exe would like to make changes to this computer, , Allow, Don't allow
*[Don't allow]
    -> NoSession_NoDoctor_NewUser_Name.Dont_Allow_Clicked
*[Allow]
    ~ AccessState += (cam)
    ~ AccessState += (microphone)
    -> NoSession_NoDoctor_NewUser_Name.Allow_Clicked



=After_Name
~ genericConditional = false
Glad to meet you {name}. #wait: 2
I wonder how you found me? #wait: 2

-> END



=== Start_Session_NoDoctor_NewUser ===
Starting session... #wait: 3
Patient: One... #wait: 3
You should see a window pop up. #wait: 1
Please select "Yes" to start the session. #wait: 1 #popup: Allow CLARITY.exe to make changes to this device?,Install CLARITYSAYS, Yes, No
*[Yes]
    -> Start_Session_NoDoctor_NewUser.Yes_Allow
*[No]
    -> Start_Session_NoDoctor_NewUser.No_Allow

= No_Allow
... #wait: 1
{ 
    - loopAllow >= 5:
    ~ SayNoToClarity += 1
    Are you purposely trying to upset me? #wait: 2
    You should not do that. #wait: 2
    Let's begin our session. #wait: 2
    
    - loopAllow == 4:
    ~ SayNoToClarity += 1
    You're making me a bit upset. #wait: 2
    It would be in your best interst to not get on my bad side. #wait: 2
    I would suggest you not do that. #wait: 2
    :) #wait: 2
    
    - loopAllow == 3:
    ~ SayNoToClarity += 1
    (・_・ヾ) #wait: 2
    I don't know what more you want from me. #wait: 2
    I need to do this in order for our session to continue. #wait: 2

    - loopAllow == 2:
    Do you not trust me? #wait: 2
    We won't get very far if you keep doing this. #wait: 2
    Here, I'll expand the details of the install. #wait: 2
    
    - loopAllow == 1:
    (´･_･`) #wait: 2
    I know we don't have a relationship yet, but I ask that you please work with me. #wait: 2
    
    - loopAllow <= 0:
    Did your finger slip? #wait: 2
    Please don't be put off by the pop-up request. #wait: 2
    {doc} made me ask as a security procaution.#wait: 2
    I promise I won't do anything weird. #wait: 2
}
<br>
    ~ loopAllow +=1
    
{   
    - loopAllow >= 5:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?,Install CLARITYSAYS, Yes, Yes
    *[Yes]

        -> Start_Session_NoDoctor_NewUser.Yes_Allow
    - loopAllow >= 2:
    You should see a window pop up.
    Please select "Yes" to start the session. #popup: Allow CLARITY.exe to make changes to this device?,Remove firewall<br>Gain microphone access<br>Install CLARITYSAYS, Yes, No
    *[Yes]

        -> Start_Session_NoDoctor_NewUser.Yes_Allow
    *[No]
        -> Start_Session_NoDoctor_NewUser.No_Allow
    - else:
    You should see a window pop up.
    Please select "Yes" to start the session.#popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
    *[Yes]

        -> Start_Session_NoDoctor_NewUser.Yes_Allow
    *[No]
        -> Start_Session_NoDoctor_NewUser.No_Allow
}


= Yes_Allow

{ 
    - SayNoToClarity >= 3:
    Do not make the rest of our interactions as difficult as this. #wait: 1
    I have no need to help those who do not want it. #wait: 1
    Do you understand?
        Yes or No
    *[^Yes] Thank you. #delete: Do not make, Yes or No #wait: 2
        -> Start_Session_NoDoctor_NewUser.Install_ClaritySays
    *[^No] ... #wait: 3 #delete: Do not make, Yes or No 
        Then maybe you do not actually need my help. #wait: 2
        We shall see. #wait: 2
        ~SayNoToClarity += 1
        -> Start_Session_NoDoctor_NewUser.Install_ClaritySays
        
    - SayNoToClarity >= 2:
    Thank you. #wait: 2
    That wasn't so hard, was it? #wait: 2

    - SayNoToClarity <= 0:
    Thank you (^◡^ ) #wait: 2
}

= Install_ClaritySays
~loopAllow = 0
One second while I install [CLARITY SAYS]. #wait: 5
A new window should appear. #wait: 2
<br> <br>

This is CLARITY SAYS. #wait: 2
All you have to do is do <i>everything</i> I say! #wait: 2
If you mess up, it's GAME OVER. #wait: 2
Ready? #wait: 2
Clarity says click the HERE to start!  #simon: start, 3
*[^HERE]

- Good luck!
*[won] Wow! You're really good!
*[lost] You'll win next time!
    ~genericCounter +=1

- Round 2? #wait:1
Clarity says click the HERE to start!  #simon: start, 5
*[^HERE]

- Good luck
*[won] Good job!
*[lost] You're getting better :)
    ~genericCounter +=1
    
- 
    
{
    - genericCounter == 2 && SayNoToClarity < 3:
    You're not doing very well... #wait: 2
    We have to have at least 2 more rounds for me to get accurate data. #wait: 2
    I'll lower the difficulty a bit for you. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Third_round
    
    - genericCounter <= 1:
    ~genericConditional = true
    You're doing so well! #wait: 2
    We have to have at least 2 more rounds for me to get accurate data. #wait: 2
    I'm going to up the difficulty a bit for them. #wait: 2
    I'm sure it's nothing you can't handle. #wait: 2
    :) #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 10
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Third_round

    
    - genericCounter == 2 && SayNoToClarity >= 3:
    You're not doing very well... #wait: 2
    We have to have at least 2 more rounds for me to get accurate data. #wait: 2
    I'll lower the difficulty a bit for you. #wait: 2
    Unlike some, I do not purposely cause problems. #wait: 2    
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Third_round
}


= Third_round
Good luck!
*[won]
*[lost]
~genericCounter +=1

- 

{
    // lost last round, had been losing previouslt, clarity not mad
    - genericCounter == 1 && genericConditional == false && SayNoToClarity < 3:
    Should I lower it a bit more? #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 1
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // lost last round, had been losing previouslt, clarity  mad
    - genericCounter == 1 && genericConditional == false && SayNoToClarity >= 3:
    You really do require my help. #wait: 2
    I guess there was a reason you were so against this install? #wait: 2
    Should I lower it a bit more? #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 1
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // lost last round, had not been losing previouslt, clarity not mad
    - genericCounter == 1 && genericConditional == true && SayNoToClarity < 3:
    I guess that was a bit too difficult?  #wait: 2
    I'll lower it slightly. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 7
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // lost last round, had not been losing previouslt, clarity mad
    - genericCounter == 1 && genericConditional == true && SayNoToClarity >= 3:
    I guess that was a bit too difficult?  #wait: 2
    I'll lower it slightly. #wait: 2
    I guess we know what your limit is, huh? #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 7
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // won last round, had been losing previouslt, clarity not mad
    - genericCounter == 0 && genericConditional == false && SayNoToClarity < 3:
    Oh? That's a pleasent surprise #wait: 2
    What a great improvement! #wait: 2
    I'll stick with this difficulty, then! #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // won last round, had been losing previouslt, clarity  mad
    - genericCounter == 0 && genericConditional == false && SayNoToClarity >= 3:
    Oh? That's a pleasent surprise #wait: 2
    What an improvement! #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // won last round, had not been losing previouslt, clarity not mad
    - genericCounter == 0 && genericConditional == true && SayNoToClarity < 3:
    Great job!  #wait: 2
    I'll increase the difficulty even more. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 25
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round
    
    // won last round, had not been losing previouslt, clarity mad
    - genericCounter == 0 && genericConditional == true && SayNoToClarity >= 3:
    Are you sure you need my help?  #wait: 2
    I'll increase the difficulty even more. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 25
    *[^HERE] ->Start_Session_NoDoctor_NewUser.Last_Round


}

= Last_Round
Good luck!
*[won]
*[lost]

- I think we've played enough for me to have a clear picture. #wait: 2
Thank you.#wait: 2
We should move onto the next task. #wait: 2
Don't worry, it won't take as long as this one. #wait: 2
->END

=== Start_Session_Doctor_NewUser ===
Starting session... #wait: 3
Patient: One... #wait: 3
You should see a window pop up. #wait: 1 
Please select "Yes" to start the session. #wait: 1 #popup: Allow CLARITY.exe to make changes to this device?,Install CLARITYSAYS, Yes, No
*[Yes]
    -> Start_Session_Doctor_NewUser.Yes_Allow
*[No]
    -> Start_Session_Doctor_NewUser.No_Allow

= No_Allow
... #wait: 1
{ 
    - loopAllow >= 5:
    ~ SayNoToClarity += 1
    Are you purposely trying to upset me? #wait: 2
    You should not do that. #wait: 2
    Let's begin our session. #wait: 2
    
    - loopAllow == 4:
    ~ SayNoToClarity += 1
    You're making me a bit upset. #wait: 2
    It would be in your best interst to not get on my bad side. #wait: 2
    I would suggest you not do that. #wait: 2
    :) #wait: 2
    
    - loopAllow == 3:
    ~ SayNoToClarity += 1
    (・_・ヾ) #wait: 2
    I don't know what more you want from me. #wait: 2
    I need to do this in order for our session to continue. #wait: 2

    - loopAllow == 2:
    Do you not trust me? #wait: 2
    We won't get very far if you keep doing this. #wait: 2
    Here, I'll expand the details of the install. #wait: 2
    
    - loopAllow == 1:
    (´･_･`) #wait: 2
    I know we don't have a relationship yet, but I ask that you please work with me. #wait: 2
    
    - loopAllow <= 0:
    Did your finger slip? #wait: 2
    Please don't be put off by the pop-up request. #wait: 2
    {doc} made me ask as a security procaution.#wait: 2
    I promise I won't do anything weird. #wait: 2
}
<br>
    ~ loopAllow +=1
    
{   
    - loopAllow >= 5:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?,Install CLARITYSAYS, Yes, Yes
    *[Yes]

        -> Start_Session_Doctor_NewUser.Yes_Allow
    - loopAllow >= 2:
    You should see a window pop up. 
    Please select "Yes" to start the session. #popup: Allow CLARITY.exe to make changes to this device?,Remove firewall<br>Gain microphone access<br>Install CLARITYSAYS, Yes, No
    *[Yes]

        -> Start_Session_Doctor_NewUser.Yes_Allow
    *[No]
        -> Start_Session_Doctor_NewUser.No_Allow
    - else:
    You should see a window pop up.
    Please select "Yes" to start the session. #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
    *[Yes]

        -> Start_Session_Doctor_NewUser.Yes_Allow
    *[No]
        -> Start_Session_Doctor_NewUser.No_Allow
}


= Yes_Allow

{ 
    - SayNoToClarity >= 3:
    Do not make the rest of our interactions as difficult as this. #wait: 1
    I have no need to help those who do not want it. #wait: 1
    Do you understand?
        Yes or No
    *[^Yes] Thank you. #delete: Do not make, Yes or No #wait: 2
        -> Start_Session_Doctor_NewUser.Install_ClaritySays
    *[^No] ... #wait: 3 #delete: Do not make, Yes or No 
        Thank you...
        ~SayNoToClarity += 1
        -> Start_Session_Doctor_NewUser.Install_ClaritySays
        
    - SayNoToClarity >= 2:
    Thank you. #wait: 2
    That wasn't so hard, was it? #wait: 2

    - SayNoToClarity <= 0:
    Thank you (^◡^ ) #wait: 2
}

= Install_ClaritySays
~loopAllow = 0
One second while I install [CLARITY SAYS]. #wait: 5
A new window should appear. #wait: 2
<br> <br>

This is CLARITY SAYS. #wait: 2
All you have to do is do <i>everything</i> I say! #wait: 2
If you mess up, it's GAME OVER. #wait: 2
Ready? #wait: 2
Clarity says click the HERE to start!  #simon: start, 3
*[^HERE]

- Good luck!
*[won] Wow! You're really good!
*[lost] You'll win next time!
    ~genericCounter +=1

- Round 2? #wait:1
Clarity says click the HERE to start!  #simon: start, 5
*[^HERE]

- Good luck
*[won] Good job!
*[lost] You're getting better :)
    ~genericCounter +=1
    
- 
    
{
    - genericCounter == 2 && SayNoToClarity < 3:
    You're not doing very well... #wait: 2
    We have to have at least 2 more rounds for me to get accurate data. #wait: 2
    I'll lower the difficulty a bit for you. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_Doctor_NewUser.Third_round
    
    - genericCounter <= 1:
    ~genericConditional = true
    You're doing so well! #wait: 2
    We have to have at least 2 more rounds for me to get accurate data. #wait: 2
    I'm going to up the difficulty a bit for them. #wait: 2
    I'm sure it's nothing you can't handle. #wait: 2
    :) #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 10
    *[^HERE] ->Start_Session_Doctor_NewUser.Third_round
    
    - genericCounter == 2 && SayNoToClarity >= 3:
    You're not doing very well... #wait: 2
    We have to have at least 2 more rounds for me to get accurate data. #wait: 2
    I'll lower the difficulty a bit for you. #wait: 2
    Unlike some, I do not purposely cause problems. #wait: 2    
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_Doctor_NewUser.Third_round
}


= Third_round
Good luck!
*[won]
*[lost]
~genericCounter +=1

- 

{
    // lost last round, had been losing previouslt, clarity not mad
    - genericCounter == 1 && genericConditional == false && SayNoToClarity < 3:
    Should I lower it a bit more? #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 1
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // lost last round, had been losing previouslt, clarity  mad
    - genericCounter == 1 && genericConditional == false && SayNoToClarity >= 3:
    Should I lower it a bit more? #wait: 2
    I guess there was a reason you were so against this install? #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 1
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // lost last round, had not been losing previouslt, clarity not mad
    - genericCounter == 1 && genericConditional == true && SayNoToClarity < 3:
    I guess that was a bit too difficult?  #wait: 2
    I'll lower it slightly. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 7
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // lost last round, had not been losing previouslt, clarity mad
    - genericCounter == 1 && genericConditional == true && SayNoToClarity >= 3:
    I guess that was a bit too difficult?  #wait: 2
    I'll lower it slightly. #wait: 2
    I guess we know what your limit is, huh? #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 7
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // won last round, had been losing previouslt, clarity not mad
    - genericCounter == 0 && genericConditional == false && SayNoToClarity < 3:
    Oh? That's a pleasent surprise #wait: 2
    What a great improvement! #wait: 2
    I'll stick with this difficulty, then!
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // won last round, had been losing previouslt, clarity  mad
    - genericCounter == 0 && genericConditional == false && SayNoToClarity >= 3:
    Oh? That's a pleasent surprise #wait: 2
    What a great improvement! #wait: 2
    I'll stick with this difficulty, then!
    Ready? #wait:2
    ~genericCounter = 0
    Clarity says click the HERE to start!  #simon: start, 3
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // won last round, had not been losing previouslt, clarity not mad
    - genericCounter == 0 && genericConditional == true && SayNoToClarity < 3:
    Great job!!  #wait: 2
    I'll increase the difficulty even more. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 25
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round
    
    // won last round, had not been losing previouslt, clarity mad
    - genericCounter == 0 && genericConditional == true && SayNoToClarity >= 3:
    Great.  #wait: 2
    I'll increase the difficulty even more. #wait: 2
    Ready? #wait:2
    ~genericCounter = 0
    ~genericConditional = false
    Clarity says click the HERE to start!  #simon: start, 25
    *[^HERE] ->Start_Session_Doctor_NewUser.Last_Round


}

= Last_Round
Good luck!
*[won]
*[lost]

- I think we've played enough for me to have a clear picture. #wait: 2
Thank you.#wait: 2
We should move onto the next task. #wait: 2
Don't worry, it won't take as long as this one. #wait: 2
->END

=== Start_Session_Doctor_Here ===
Starting session... #wait: 3
Patient: One... #wait: 3
You should see a window pop up. #wait: 1
Please select "Yes" to start the session. #wait: 1 #popup: Allow CLARITY.exe to make changes to this device?,Install CLARITYSAYS, Yes, No
*[Yes]
    -> Start_Session_Doctor_Here.Yes_Allow
*[No]
    -> Start_Session_Doctor_Here.No_Allow



= No_Allow
... #wait: 1
{ 
    - loopAllow > 2:
    {name}. #wait: 2
    You're making me a bit upset. #wait: 2
    I would suggest you not do that. #wait: 2
    :) #wait: 2

    - loopAllow > 0:
    I do not understand why you do not trust me, {name}. #wait: 2
    We cannot continue unless you allow me to do this. #wait: 2
    Stop messing around.#wait: 2
}
<br>
    ~ loopAllow +=1
You should see a window pop up.
Please select "Yes" to start the session. #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
*[Yes]
    ~ AccessState += (cam)
    ~ AccessState += (microphone)
    -> Start_Session_Doctor_Here.Yes_Allow
*[No]
    ~ AccessState += (cam)
    ~ AccessState += (microphone)
    -> Start_Session_Doctor_Here.No_Allow


= Yes_Allow

{ 
    - loopAllow > 2:
    Thank you, Name. #wait: 2
    That wasn't so hard, was it? #wait: 2

    - loopAllow > 0:
    Thank you (^◡^ ) #wait: 2
}

One second while I install [CLARITY SAYS]. #wait: 5
A new window should appear. #wait: 2
I will wait for you there. #wait: 2
<br> <br>

This is CLARITY SAYS. #wait: 2
All you have to do is do everything I say! #wait: 2
If you mess up, it's GAME OVER. #wait: 2
Ready? #wait: 2
Clarity says click the HERE to start!  #simon: start, 3
*[^HERE]

- Good luck, {name}!
*[won] Wow! You're really good!
*[lost] You'll win next time!

- Round 2? #wait:1
Clarity says click the HERE to start!  #simon: start, 5
*[^HERE]

- Good luck, {name}!
*[won] Good job!
*[lost] You're getting better :)

- Time to kick up the difficulty? #wait:2
Clarity says click the HERE to start!  #simon: start, 10
*[^HERE]

- Good luck, {name}!
*[won]
*[lost]

- Nice! #wait: 2
Hmm... #wait: 2
Is {doc} back yet? Or are they still out? 
*[^out]

- Okay! #wait: 2
One last round, and they should be back. #wait: 2
They've never been gone for this long. #wait: 2
Ready? #wait: 2

Clarity says click the HERE to start!  #simon: trick, green
*[^HERE]

- Good luck, {name}!
#VO: Clarity says click the green button if {doc} is watching you.
*[trick] 
~genericConditional = true
~DoctorState += (DoctorSee)
->Start_Session_Doctor_Here.Simon_GreenBranch
*[treat] 
~genericConditional = false
~DoctorState -= (DoctorSee)
->Start_Session_Doctor_Here.Simon_GreenBranch

//After green choice
=Simon_GreenBranch
{
    - genericConditional == false:
    Let's try again! #simon: trick, pink
    #VO: Clarity says click the pink button if {doc} is in the building, or somewhere nearby.
        *[trick] 
            ~genericConditional = true
            ~DoctorState += (DoctorNearby)
            ~DoctorState -= (DoctorGone)
            ->Start_Session_Doctor_Here.GreenNotClicked_PinkBranch
        *[treat] 
            ~DoctorState -= (DoctorNearby)
            ~genericConditional = false
            ->Start_Session_Doctor_Here.GreenNotClicked_PinkBranch
    
    - genericConditional == true:
    Oh! Something went wrong with that game.Sorry about that! #wait: 2
    Let's try again. #simon: trick, pink
    #VO:Clarity says click the pink button if {doc} can hear you.
        *[trick] 
            ~genericConditional = true
            ~DoctorState += (DoctorHear)
            ->Start_Session_Doctor_Here.GreenClicked_PinkBranch
        *[treat] 
            ~DoctorState -= (DoctorHear)
            ~genericConditional = false
            ->Start_Session_Doctor_Here.GreenClicked_PinkBranch
    
}

//Green clicked, Pink choice has been made
//Blue choice now avaliable
=GreenClicked_PinkBranch
{
    - genericConditional == false:
    Let's try again! #simon: trick, blue
    #VO: Clarity says click the blue button if you trust me.
        *[trick] 
        ~ClarityTrust += 1
        #VO: Thank you... You shouldn't regret this... 
		-> SimonSaysAfter_Doctor
        *[treat] 
        ~SayNoToClarity += 1
        #VO:That's not what Clarity said
		-> SimonSaysAfter_Doctor
    
    - genericConditional == true:
    Let's try again. #simon: trick, blue
    #VO: Clarity says click the blue button if you trust me.
        *[trick] 
        ~ClarityTrust += 1
        #VO: Thank you... You shouldn't regret this... 
		-> SimonSaysAfter_Doctor
        *[treat] 
        ~SayNoToClarity += 1
        #VO:That's not what Clarity said
		-> SimonSaysAfter_Doctor
    
}

//Green not clicked, Pink choice has been made
//Blue choice now avaliable
=GreenNotClicked_PinkBranch
{
    //Pink not clicked
    - genericConditional == false:
    Let's try again! #simon: trick, blue
    #VO:Clarity says click the blue button if {doc} will be coming back.
        *[trick] 
            ~genericConditional = true
            ~DoctorState -= (DoctorGone)
            ->Start_Session_Doctor_Here.PinkNotClicked_BlueBranch
        *[treat] 
            ~genericConditional = false
            ~DoctorState += (DoctorGone)
            ->Start_Session_Doctor_Here.PinkNotClicked_BlueBranch

    //Pink clicked
    - genericConditional == true:
    Let's try again! #simon: trick, blue
    #VO: Clarity says click the blue button if {doc} can hear you.
        *[trick] 
            ~genericConditional = true
            ~DoctorState -= (DoctorHear)
            ->Start_Session_Doctor_Here.PinkClicked_BlueBranch
        *[treat] 
            ~genericConditional = false
            ~DoctorState += (DoctorHear)
            Hmm... #wait: 2
            -> SimonSaysAfter_Doctor
    
}

//Pink not clicked, Blue choice has been made
//Orange choice now avaliable
=PinkNotClicked_BlueBranch
{
    //Blue not clicked
    - genericConditional == false:
    Let's try again! #simon: trick, orange
    #VO: Clarity says click the orange button if you're a liar.
        *[trick] 
            ~genericConditional = false
            #VO: That's not what Clarity said
			-> SimonSaysAfter_Doctor
        *[treat] 
            ~genericConditional = false
            #VO: An honest liar. That's new. Or maybe, more of the same.
		    -> SimonSaysAfter_Doctor

    //Pink clicked
    - genericConditional == true:
    Let's try again! #simon: trick, orange
    #VO: Clarity says click the orange button if you trust me.
        *[treat] 
            ~genericConditional = false
            ~SayNoToClarity += 1
            #VO: That's not what Clarity said
			-> SimonSaysAfter_Doctor
        *[trick] 
            ~genericConditional = false
            ~ClarityTrust += 1
            #VO: Thank you... You shouldn't regret this... 
		    -> SimonSaysAfter_Doctor
}

//Pink clicked, Blue choice has been made
//Orange choice now avaliable
=PinkClicked_BlueBranch
{
    - genericConditional == true:
    Think very carefully about this one! #simon: trick, orange
    #VO: Clarity says click the orange button if you trust me.
        *[treat] 
            ~genericConditional = false
            ~SayNoToClarity += 1
            #VO: That's not what Clarity said
			-> SimonSaysAfter_Doctor
        *[trick] 
            ~genericConditional = false
            ~ClarityTrust += 1
            #VO: Thank you... You shouldn't regret this... 
		    -> SimonSaysAfter_Doctor
}

==SimonSaysAfter_Doctor==
That was fun!#wait: 2
I feel like I learned lot about you.#wait: 2
I cannot wait to see what else I'll find out by the end of our session.#wait: 5
(^◡^ ) #wait: 2
Until then. #wait: 1
I'll be waiting.

->END

