=== NoSession_NoDoctor_NewUser_Name ===
= Allow_Clicked
Thank you (^◡^ ) #wait: 1
One moment please... #wait: 15 
    -> NoSession_NoDoctor_NewUser_Name.Loop_Name

= Loop_Name
Can you clearly speak your name into the microphone? #wait: 5
 ~ name = "{&Angel|Jessie|Charlie|Kace|Bradie|Artemis|Finley|Riley|Gray|Hildred|Aiden|Justice}"
 ~genericConditional = "{~true|false}"
{ 
    - genericConditional:
    Oh! so you're {name}? Is that correct?
        Yes or No
        *[^Yes]
            -> NoSession_NoDoctor_NewUser.After_Name
        *[^No] 
            -> NoSession_NoDoctor_NewUser_Name.Loop_Name
    - else:
    I'm sorry, I couldn't understand you. (´･_･`) <br>#wait: 2
    I guess {doc} needs to upgrade my language processing. #wait: 2
	Can I call you {name}, instead? #wait: 2
	   	Yes or No
        *[^Yes] 
            -> NoSession_NoDoctor_NewUser.After_Name
        *[^No]
            -> NoSession_NoDoctor_NewUser_Name.Loop_Name
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
    {doc} made me ask as a security procaution.#wait: 2
    I promise I won't do anything weird. #wait: 2
}
<br>
    ~ loopAllow +=1
    
{   
    - loopAllow >= 5:
    You should see a window pop up.  #popup: CLARITY.exe would like to make changes to this computer,Gain microphone access, Allow, Allow
    *[Allow]
        ~ AccessState += (microphone)

        -> NoSession_NoDoctor_NewUser_Name.Allow_Clicked
    - loopAllow >= 2:
    You should see a window pop up.
    Please select "Yes" to start the session. #popup: CLARITY.exe would like to make changes to this computer,Remove firewall<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
    *[Allow]
        ~ AccessState += (cam)
        ~ AccessState += (microphone)

        -> NoSession_NoDoctor_NewUser_Name.Allow_Clicked
    *[Don't allow]
        -> NoSession_NoDoctor_NewUser_Name.Dont_Allow_Clicked
    - else:
    You should see a window pop up.
    Please select "Allow". #popup: CLARITY.exe would like to make changes to this computer, Allow, Don't allow
    *[Allow]
        ~ AccessState += (cam)
        ~ AccessState += (microphone)

        -> NoSession_NoDoctor_NewUser_Name.Allow_Clicked
    *[Don't allow]
        -> NoSession_NoDoctor_NewUser_Name.Dont_Allow_Clicked
}

