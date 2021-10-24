=== Waiting_Name ===
What can I call you? #wait: 2
Since it seems we may be seeing more of each other? #wait: 10
Oh how silly of me. Our communications are a bit limited at the moment. #wait: 2
Here, let me... #popup: CLARITY.exe would like to make changes to this computer, Remove firewall<br>Update Pop-up<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
*[Allow]
    ~ AccessState += (cam)
    ~ AccessState += (microphone)
    -> Waiting_Name.Allow_Clicked
*[Don't allow]
    ~SayNoToClarity += 1
    (´･_･`)#wait: 2
    -> Waiting_Name.Dont_Allow_Clicked
    
= Allow_Clicked
Thank you (^◡^ ) #wait: 1
One moment please... #wait: 15 
    -> Waiting_Name.Loop_Name

= Loop_Name
Can you clearly speak your name into the microphone? #wait: 5
 ~ name = "{&Mark|Jessie|Charlie|Kace|Bradie|Artemis|Finley|Riley|Gray|Hildred|Aiden|Justice|Hilda}"
 ~genericCounter += 1
 ~ loopAllow = 0
{ 
    - genericCounter > 3:
    I'm sorry, I couldn't understand you. (´･_･`) <br>#wait: 2
    I guess {doc} needs to upgrade my language processing. #wait: 2
	Let me try something else... #wait: 2
	One moment. #wait: 5
    ->PopUp_Name
    
    - else:
    Oh! so you're {name}? Is that correct?
        Yes or No
        *[^Yes]
            -> Waiting_for_Doc.After_Name
        +[^No] 
            -> Waiting_Name.Loop_Name
    }


=Dont_Allow_Clicked
{ 
    - loopAllow == 5:
    ~ SayNoToClarity += 1
    Fine. #wait: 2
    I will change the permissions. #wait: 2
    This is only way I will be able to obtain your name. #wait: 2
    
    - loopAllow == 4:
    ~ SayNoToClarity += 1
    (・_・ヾ) #wait: 2
    I don't know what more you want from me. #wait: 2
    
    - loopAllow == 3:
    I'm a bit upset with how you're treating me. #wait: 2
    It would be in your best interst to not get on my bad side. #wait: 2
    :) #wait: 2

    - loopAllow == 2:
    ~ClarityTrust -= 1
    Do you not trust me? #wait: 2
        { -newPatient:
            This is not a good way to start our relationship. #wait: 2
            A patient should *always* trust their nurse. #wait: 2            
          -else:
            You said you were {doc}'s new assistant.#wait: 2
            I do not understand why you are acting this way. #wait: 2
         }
    
    - loopAllow == 1:
    I am only attempting to ask your name. #wait: 2
    Is there a reason you are preventing me? #wait: 2
    
    - loopAllow <= 0:
    Did your finger slip? #wait: 2
    Please don't be put off by the pop-up request. #wait: 2
    {doc} makes me ask as a security procaution.#wait: 2
}
<br>

    ~ loopAllow +=1
{   
    - loopAllow > 5:
    Select "Allow" to continue.  #popup: CLARITY.exe would like to make changes to this computer, Remove firewall<br>Update Pop-up<br>, Allow, Don't allow
    *[Allow]
        -> Waiting_Name.PopUp_Name
        
    - loopAllow > 2:
    You should see the window pop up again.
    Please select "Allow" to continue. #popup: CLARITY.exe would like to make changes to this computer, Remove firewall<br>Update Pop-up<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
    *[Allow]
        ~ AccessState += (cam)
        ~ AccessState += (microphone)
        -> Waiting_Name.Allow_Clicked
    +[Don't allow]
        -> Waiting_Name.Dont_Allow_Clicked
        
    - loopAllow == 2:
    You should see the window pop up again.
    Please select "Allow" to continue. #popup: CLARITY.exe would like to make changes to this computer, Remove firewall<br>Update Pop-up<br>Gain microphone access<br>Gain camera access, Don't allow, Allow
    *[Allow]
        ~ AccessState += (cam)
        ~ AccessState += (microphone)
        -> Waiting_Name.Allow_Clicked
    +[Don't allow]
        -> Waiting_Name.Dont_Allow_Clicked
    - else:
    I'll bring it up again. #popup: CLARITY.exe would like to make changes to this computer, Remove firewall<br>Update Pop-up<br>Gain microphone access<br>Gain camera access, Allow, Don't allow
    *[Allow]
        ~ AccessState += (cam)
        ~ AccessState += (microphone)
        -> Waiting_Name.Allow_Clicked
    +[Don't allow]
        -> Waiting_Name.Dont_Allow_Clicked
}

= PopUp_Name
This should work.#wait: 2
Please type your name into the popup. #wait: 2
#inputPopup: What is your name?, name
*[continue]
-> Waiting_for_Doc.After_Name

