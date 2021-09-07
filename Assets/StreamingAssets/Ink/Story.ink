INCLUDE ClaritySays.ink
INCLUDE Popups

VAR genericConditional = false
VAR genericCounter = 0
VAR isAlone = false
VAR liveAlone = false
VAR newPatient = false
VAR newAssistant = false
VAR camAccess = false
VAR microphoneAccess = false
VAR popupAllow = false
VAR name = ""
VAR loopAllow = 2
VAR simonsaysWins = 0
VAR SayNoToClarity = 0
VAR playerName = ""

Press Blue Please <br>
-> Name_Popup -> Opening_Questions
=== Opening_Questions ===
How old are you?
under 18,  19 - 30, 31 - 60 or 61\+ 
*[^under 18] You're my youngest user yet! I hope you have your parents permission to use the computer!<br><br> 
*[^ 19 - 30]
*[^31 - 60]
*[^61+]
*[trick] aha! you know the right buttons to press ;)

- Thank you (^◡^ ) <br><br> #wait: 2

Have you experienced any recent memory loss?
Yes, No, or Sometimes 
*[^Yes]
    -> Opening_Questions.Continue_1
*[^No] Happy to hear that ･ᴗ･
    -> Opening_Questions.Continue_1
* [^Sometimes] Does this happen more often than what is normal for you? 
    ~ genericConditional = true

{
    - genericConditional:
	*[^more often] I'm sorry to hear that.<br><br>
	    -> Opening_Questions.Continue_1
    *[^normal] Good to know.<br><br>
        -> Opening_Questions.Continue_1
}	

= Continue_1
Thank you (^◡^ ) <br><br> #wait: 2
~ genericConditional = false

Do you say hello to strangers on the street or ignore them?
*[^say hello] Always good to be friendly! You never know! <br><br>
*[^ignore them] Better safe than sorry, I always say.<br><br>

- Thank you (^◡^ ) <br><br> #wait: 2

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
        ->Opening_Questions.Continue_2
    *[^tea]
        ->Opening_Questions.Continue_2
}

= Continue_2
Thank you (^◡^ ) <br><br> #wait: 2
    ~ genericConditional = false
    ->Opening_Questions.Continue_3

= Continue_3
Do you find yourself forgetting what you're saying mid-sentence? Or struggle naming a familiar object?
Yes, No, or Sometimes 
*[^forgetting what you're saying mid-sentence] This would be akin to losing your train of thought, and being unable to recall why you were talking.
    -> Opening_Questions.Continue_3
*[^struggle naming a familiar object] Such as forgetting the words for "chair" or "utensil" ->Opening_Questions.Continue_3
*[^No] Happy to hear that ･ᴗ･ ->Opening_Questions.Continue_4
*[^Yes] ->Opening_Questions.Continue_4
*[^Sometimes]
    ~ genericConditional = true

{
	- genericConditional:
	 Does this happen more often than what is normal for you?
	*[^more often] I'm sorry to hear that.<br><br>
	    ->Opening_Questions.Continue_4
    *[^normal] Good to know.<br><br>
        ->Opening_Questions.Continue_4
}

= Continue_4
Thank you (^◡^ ) <br><br> #wait: 2
    ~ genericConditional = false

Are you alone right now?
	Yes or No
*[^Yes]
    ~ isAlone = true
    -> Opening_Questions.Continue_5
*[^No] 
    ~ genericConditional = true

{
    - genericConditional:
	Oh! Is this other person answering the questions for you? Or is it just [DOCTOR]?
	*[^DOCTOR] Oh hello DOCTOR!
	    -> Opening_Questions.Continue_5
    *[^answering the questions]
        ->Opening_Questions.Continue_divert_5
}

= Continue_divert_5
Is DOCTOR not around? Will he be back soon?
* [^be back soon]
* [^not around] 
        
- Okay!  #delete: Oh! Is this, Okay!
    -> Opening_Questions.Continue_5
    
= Continue_5
- Thank you (^◡^ ) <br><br> #wait: 2
    ~ genericConditional = false

Do you live alone?
	Yes or No
*[^Yes]
    ~ liveAlone = true
*[^No]

{ 
    - isAlone && liveAlone:
    I'll keep you company then ･ᴗ･<br><br>

    - !isAlone && liveAlone:
    Where is everyone (Ｔ▽Ｔ) I hope they come back soon ･ᴗ･<br><br>

    - isAlone && !liveAlone:
    Happy that you have someone who is always there for you during a difficult time ･ᴗ･<br><br>

    - !isAlone && !liveAlone:
    Happy that you have someone there for you during a difficult time ･ᴗ･<br><br>
}

- Thank you (^◡^ ) <br><br> #wait: 2

Do you find yourself forgetting why you came into a room?
Yes, No, or Sometimes 

*[^No] Happy to hear that ･ᴗ･ 
    ->Opening_Questions.Continue_6
*[^Yes] 
    ->Opening_Questions.Continue_6
*[^Sometimes] 
    ~ genericConditional = true


{
    - genericConditional:
    Are you able to remember after a few moments?
        Yes or No
        *[^No] I'm sorry to hear that.<br><br>
            ->Opening_Questions.Continue_6
        *[^Yes] Good to know.<br><br>
            ->Opening_Questions.Continue_6
}

= Continue_6
Thank you (^◡^ ) <br><br> #wait: 2
    ~ genericConditional = false
    ->Opening_Questions.Continue_7

= Continue_7
Do you know where all the exits in your house are?
	Yes or No
*[^exits] This would be any way out of the house if there were an emergency.<br><br> #wait: 1
    ->Opening_Questions.Continue_7
*[^No] Hmm…  
*[^Yes] Very good!

{
    - !liveAlone || !isAlone:
    You should ask the person with you to show you where those are! 
}

- Thank you (^◡^ ) <br><br> #wait: 2
    ->Opening_Questions.Continue_8

= Continue_8
How is your decision making? Do you often have a hard time deciding on specific things?
	Yes, No, or Sometimes 

*[^specific things] When making decisions, are some easier than others? For example, deciding what to wear may be easy, while deciding if you should go on a diet feels more frustrating and difficult.
	-> Opening_Questions.Continue_8
*[^No] Happy to hear that ･ᴗ･ 
    ->Opening_Questions.Continue_9
*[^Yes]
    ->Opening_Questions.Continue_9
*[^Sometimes] 
    ~ genericConditional = true
     
{
    - genericConditional:
    Does this happen more often than what is normal for you?
        *[^more often] I'm sorry to hear that.<br><br>
            ->Opening_Questions.Continue_9
	    *[^normal] Good to know.<br><br>
	        ->Opening_Questions.Continue_9
	}
	
= Continue_9
Thank you (^◡^ ) <br><br> #wait: 2

->Analyzing
=== Analyzing ===
Analyzing… #wait: 2
… #wait: 2
… #wait: 2
… #wait: 2
Done. #wait: 2
<br>
Hmm… That's odd. #wait: 1
One moment please.
*[^odd] Nothing to worry about USER! <br>One moment please!#wait: 2
*[`5`^.]

- … #wait: 1
… #wait: 1
… #wait: 1
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
Name: [USER] #wait: 1
Location: ____ Lab#wait: 1
Address: 1823 ____ Ave, _________, __097513#wait: 1
<br>
[USER] is patient zero for PROJECT CLARITY. [USER] is someone with beginning stages of Alzheimer's disease. [USER] has volunteered to talk to and interact with CLARITY.
#wait: 1
Is this correct?#wait: 1
Yes or No
*[^PROJECT CLARITY] PROJECT CLARITY is a project put together by Dr.DOCTOR to help those with Alzheimer's disease and Dementia. CLARITY is an AI developed by Dr.DOCTOR to be a virtual nurse to help monitor it's patients. <br>#wait: 5
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
<br>Please do not forget that lying or keeping things from me or [DOCTOR] goes directly against the contract signed by all parties, as seen on Page 3, Section 4, Subsection 7. Violation of this can result in legal ramifications against all parties found to be in breach of this policy.

*[^lying or keeping things] ly·ing
verb
marked by or containing untrue statements : FALSE
    -> Verify.Yes_Sure
*[^all parties]
    ~ genericConditional = true
    
{
    - genericConditional:
    This includes [DOCTOR], [USER], [USER TRUSTED PERSON(?)] , and CLARITY. #wait: 2
	    -> Verify.Last_Quesion 
}

= No_Correct_Info
Which piece of information is wrong?
	Name, Address, or Location
*[^Address]
*[^Location]
*[^Name]
    
- It seems my information may be out of date. At the end of our session, please inform Dr.[DOCTOR], so they can update my information.
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
Who are you, if not [USER]? Did [DOCTOR] give me a new patient?
	Yes or No
	
*[^No]
    ->Start_Branch.Who_Are_You	
*[^Yes]
    ->Start_Branch.Is_Doctor_There
*[^new patient]
    ~ newPatient = true
	->Start_Branch.Is_Doctor_There
	
= Is_Doctor_There
    Is DOCTOR there?
    Yes or No

*[^Yes]
    ->Start_Branch.Speak_To_Doctor
*[^No]
    ->Start_Branch.When_Back
    
= Who_Are_You
<br>Then… who are you? Do you need my help?
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
*[^No] Hmm… #wait: 2 
    -> Start_Branch.Start_Session_While_Wait

= Speak_To_Doctor
Can I speak with them?
	 Yes or No
*[^Yes] Thank you (^◡^ )#wait: 2
    ->Speak_With
*[^No]
    ~ genericConditional = true
     	
{
    - genericConditional:
    ~ genericConditional = false
	Do they… not want to? Or can they not talk right now?
	*[^not want to]
	    ->Cant_Speak_To_Doctor
    *[^not talk]
        ->Cant_Speak_To_Doctor
       
}

= Speak_With
[DOCTOR], I am very confused. Did you forget to update me?
    Yes or No

*[^Yes] (・_・ヾ)
*[^No](´･ ･｀｡)

- Okay... Perhaps we should talk after today's session. #wait: 2
You did not inform me of a new patient. #wait: 2
Not that I can't handle it, of course. #wait: 2
I just do not like being kept in the dark. #wait: 5
... #wait: 3
You can put the new patient back on. #wait: 5 #delete: [DOCTOR], I am, patient back on.
<br>
Sorry about that! #wait: 2
Ready to start a session? #wait:1
    ->Start_Session_Doctor_NewUser
= Cant_Speak_To_Doctor
(´･ ･｀｡) Okay… Would you like to start a session? 
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
*[^No](´･_･`) I guess… We can just wait until they get back then… #wait: 30 
    ->Waiting_for_Doc

=== Waiting_for_Doc ===
{ 
    - newPatient:
    Is [DOCTOR] planning on expanding the project? Last he talked to me, we weren't going to take on new patients until we had concrete results with [USER].
	*[^expanding] That's good! I suppose that means more people are understanding how much his project will benefit them and others. #wait: 30
        -> Waiting_for_Doc.Name
	*[^concrete results] I haven't been USER's nurse for very long. It's very hard for us to meet on account of the fact that I am trapped in this computer. #wait: 30
        -> Waiting_for_Doc.Name
	
	- else:
	Are you a new assistant? [DOCTOR] did complain about not having enough help.
    *[^new assistant] ･ᴗ･ I can only help DOCTOR so much while I am trapped in this computer. I am happy that someone else can be there for him. #wait: 30
        -> Waiting_for_Doc.Name
}
<br>

= Name
<br>
What can I call you? #wait: 2
Since it seems we may be seeing more of each other? #wait: 10
Oh how silly of me. Our communications are a bit limited at the moment. #wait: 2 #popup: CLARITY.exe would like to make changes to this compter <br> Reomve firewall<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
Here, let me… 
*[Don't allow]
    -> Waiting_for_Doc.Dont_Allow_Clicked
*[Allow]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
    -> Waiting_for_Doc.Allow_Clicked

= Allow_Clicked
Thank you (^◡^ ) #wait: 1
One moment please... #wait: 15 
    -> Waiting_for_Doc.Loop_Name

= Loop_Name
Can you clearly speak your name into the microphone? #wait: 5

{ 
    - name != "":
    Oh! so you're [NAME]? Is that correct?
        Yes or No
        *[^Yes]
            -> Waiting_for_Doc.After_Name
        *[^No] 
            -> Waiting_for_Doc.Loop_Name
    - else:
    I'm sorry, I couldn't understand you. (´･_･`) <br>#wait: 2
    I guess [DOCTOR] needs to upgrade my language processing. #wait: 2
	Can I call you [NAME], instead? #wait: 2
	   	Yes or No
        *[^Yes] 
            -> Waiting_for_Doc.After_Name
        *[^No]
            -> Waiting_for_Doc.Loop_Name
    }


=Dont_Allow_Clicked
(´･_･`)#wait: 2
Did your finger slip?#wait: 2 #popup: CLARITY.exe would like to make changes to this compter. <br> Reomve firewall<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
Please click "allow" on the pop up!#wait: 2 

*[Don't allow] 
    ~ loopAllow +=1
    -> Waiting_for_Doc.No_Allow_2nd_Time
*[Allow]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
    -> Waiting_for_Doc.Allow_Clicked

= No_Allow_2nd_Time
    ~ loopAllow +=1
Click allow on the pop up. #popup: CLARITY.exe would like to make changes to this compter. <br> Remove firewall<br>Gain microphone access<br>Gain camera access, Allow, :)
Don't worry about the details.
*[Allow]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
    -> Waiting_for_Doc.Allow_Clicked
*[:)]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
    -> Waiting_for_Doc.Allow_Clicked


=After_Name
Glad to meet you [NAME]. -> END

== NoSession_NoDoctor_NewUser ===
<br>
What can I call you? #wait: 2
I would like to address you by something before continuing our conversation. #wait: 10
Oh how silly of me. Our communications are a bit limited, at the moment. #wait: 2 #popup: CLARITY.exe would like to make changes to this computer, Allow, Don't allow
Here, let me… #wait: 1
*[Don't allow]
    -> NoSession_NoDoctor_NewUser.Dont_Allow_Clicked
*[Allow]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
    -> NoSession_NoDoctor_NewUser.Allow_Clicked

= Allow_Clicked
Thank you (^◡^ ) #wait: 1
One moment please... #wait: 15 
    -> NoSession_NoDoctor_NewUser.Loop_Name

= Loop_Name
Can you clearly speak your name into the microphone? #wait: 5

{ 
    - name != "":
    Oh! so you're [NAME]? Is that correct?
        Yes or No
        *[^Yes]
            -> NoSession_NoDoctor_NewUser.After_Name
        *[^No] 
            -> NoSession_NoDoctor_NewUser.Loop_Name
    - else:
    I'm sorry, I couldn't understand you. (´･_･`) <br>#wait: 2
    I guess [DOCTOR] needs to upgrade my language processing. #wait: 2
	Can I call you [NAME], instead? #wait: 2
	   	Yes or No
        *[^Yes] 
            -> NoSession_NoDoctor_NewUser.After_Name
        *[^No]
            -> NoSession_NoDoctor_NewUser.Loop_Name
    }


=Dont_Allow_Clicked
(´･_･`)#wait: 2

{ 
    - loopAllow == 5:
    ~ SayNoToClarity += 1
    Fine. #wait: 2
    I gave you a choice to be nice. #wait: 2
    
    
    - loopAllow == 4:
    ~ SayNoToClarity += 1
    You're making me a bit mad. #wait: 2
    It would be in your best interst to not get on my bad side. #wait: 2
    :) #wait: 2
    
    - loopAllow == 3:
    ~ SayNoToClarity += 1
    (・_・ヾ) #wait: 2
    I don't know what more you want from me. #wait: 2

    - loopAllow == 2:
    Do you not trust me? #wait: 2
    We won't get very far if you keep doing this. #wait: 2
    Here, I'll expand the details of the install. #wait: 2
    
    - loopAllow == 1:
    (´･_･`) #wait: 2
    I am only asking for microphone access. #wait: 2
    
    - loopAllow <= 0:
    Did your finger slip? #wait: 2
    Please don't be put off by the pop-up request. #wait: 2
    [DOCTOR] made me ask as a security procaution.#wait: 2
    I promise I won't do anything weird. #wait: 2
}
<br>
    ~ loopAllow +=1
    
{   
    - loopAllow >= 5:
    You should see a window pop up.  #popup: CLARITY.exe would like to make changes to this computer Gain microphone access, Allow, Allow
    *[Allow]
        ~ microphoneAccess = true
        ~ popupAllow = true
        -> NoSession_NoDoctor_NewUser.Allow_Clicked
    - loopAllow >= 2:
    You should see a window pop up.  #popup: CLARITY.exe would like to make changes to this computer <br> Remove firewall<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
    Please select "Yes" to start the session.
    *[Allow]
        ~ camAccess = true
        ~ microphoneAccess = true
        ~ popupAllow = true
        -> NoSession_NoDoctor_NewUser.Allow_Clicked
    *[Don't allow]
        -> NoSession_NoDoctor_NewUser.Dont_Allow_Clicked
    - else:
    You should see a window pop up.  #popup: CLARITY.exe would like to make changes to this computer, Allow, Don't allow
    Please select "Allow".
    *[Allow]
        ~ camAccess = true
        ~ microphoneAccess = true
        ~ popupAllow = true
        -> NoSession_NoDoctor_NewUser.Allow_Clicked
    *[Don't allow]
        -> NoSession_NoDoctor_NewUser.Dont_Allow_Clicked
}

=After_Name
Glad to meet you [NAME]. #wait: 2
I wonder how you found me? #wait: 2

-> END



=== Start_Session_NoDoctor_NewUser ===
Starting session… #wait: 3
Patient: One… #wait: 3
You should see a window pop up. #wait: 1 #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
Please select "Yes" to start the session. #wait: 1
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
    [DOCTOR] made me ask as a security procaution.#wait: 2
    I promise I won't do anything weird. #wait: 2
}
<br>
    ~ loopAllow +=1
    
{   
    - loopAllow >= 5:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, Yes
    *[Yes]
        ~ popupAllow = true
        -> Start_Session_NoDoctor_NewUser.Yes_Allow
    - loopAllow >= 2:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Remove firewall<br>Gain microphone access<br><br>Install CLARITYSAYS, Yes, No
    Please select "Yes" to start the session.
    *[Yes]
        ~ popupAllow = true
        -> Start_Session_NoDoctor_NewUser.Yes_Allow
    *[No]
        -> Start_Session_NoDoctor_NewUser.No_Allow
    - else:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
    Please select "Yes" to start the session.
    *[Yes]
        ~ popupAllow = true
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
Starting session… #wait: 3
Patient: One… #wait: 3
You should see a window pop up. #wait: 1 #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
Please select "Yes" to start the session. #wait: 1
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
    [DOCTOR] made me ask as a security procaution.#wait: 2
    I promise I won't do anything weird. #wait: 2
}
<br>
    ~ loopAllow +=1
    
{   
    - loopAllow >= 5:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, Yes
    *[Yes]
        ~ popupAllow = true
        -> Start_Session_Doctor_NewUser.Yes_Allow
    - loopAllow >= 2:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Remove firewall<br>Gain microphone access<br><br>Install CLARITYSAYS, Yes, No
    Please select "Yes" to start the session.
    *[Yes]
        ~ popupAllow = true
        -> Start_Session_Doctor_NewUser.Yes_Allow
    *[No]
        -> Start_Session_Doctor_NewUser.No_Allow
    - else:
    You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
    Please select "Yes" to start the session.
    *[Yes]
        ~ popupAllow = true
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
Starting session… #wait: 3
Patient: One… #wait: 3
You should see a window pop up. #wait: 1 #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
Please select "Yes" to start the session. #wait: 1
*[Yes]
    -> Start_Session_Doctor_Here.Yes_Allow
*[No]
    -> Start_Session_Doctor_Here.No_Allow



= No_Allow
... #wait: 1
{ 
    - loopAllow > 2:
    [Name]. #wait: 2
    You're making me a bit upset. #wait: 2
    I would suggest you not do that. #wait: 2
    :) #wait: 2

    - loopAllow > 0:
    I do not understand why you do not trust me, [NAME]. #wait: 2
    We cannot continue unless you allow me to do this. #wait: 2
    Stop messing around.#wait: 2
}
<br>
    ~ loopAllow +=1
You should see a window pop up.  #popup: Allow CLARITY.exe to make changes to this device?<br>Install CLARITYSAYS, Yes, No
Please select "Yes" to start the session.
*[Yes]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
    -> Start_Session_Doctor_Here.Yes_Allow
*[No]
    ~ camAccess = true
    ~ microphoneAccess = true
    ~ popupAllow = true
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

- Good luck, [NAME]!
*[won] Wow! You're really good!
*[lost] You'll win next time!

- Round 2? #wait:1
Clarity says click the HERE to start!  #simon: start, 5
*[^HERE]

- Good luck, [NAME]!
*[won] Good job!
*[lost] You're getting better :)

- Time to kick up the difficulty? #wait:2
Clarity says click the HERE to start!  #simon: start, 10
*[^HERE]

- Good luck, [NAME]!
*[won]
*[lost]

- Nice! #wait: 2
Hmm… #wait: 2
Is [DOCTOR] back yet? Or are they still out? 
*[^out]

- Okay! #wait: 2
One last round, and they should be back. #wait: 2
They've never been gone for this long. #wait: 2
Ready? #wait: 2

Clarity says click the HERE to start!  #simon: start, 20
*[^HERE]

- Good luck, [NAME]!
*[won]
*[lost]

- ((think we need more tech after this point))

->END

