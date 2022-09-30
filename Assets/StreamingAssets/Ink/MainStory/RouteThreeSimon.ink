=== SimonDownload ===

= DontAllow
{
    - SayNoToClarity <= 0:
        ~ SayNoToClarity+=0.5
        (´･ ･｀｡)
        
    - else:
        (・_・ヾ)
        
        {
            - lastchance:
                ~ SayNoToClarity+=1
                {name}, did you not remember my warning?
                I tried to be nice, despite your actions. #VO: And now I am out of patience.
                Click. Allow. Now. #popup: CLARITY.exe would like to make changes to this computer,Make Clarity an Admin, Allow, Allow
                    *[Allow]
                    ~ AccessState += (FireWall_One)
                    ~ AccessState += (FireWall_Two)
                    ~ AccessState += (FireWall_Three)
                    ->Route3_Endings.LastChanceEnding
                    *[Allow]
                    ~ AccessState += (FireWall_One)
                    ~ AccessState += (FireWall_Two)
                    ~ AccessState += (FireWall_Three)
                    ->Route3_Endings.LastChanceEnding
        
        }
}
//max SayNoToClarity can be by this point is 1.5, loweest is 0.5
{name}...
I will give you the benefit of the doubt here.
{
    - SayNoToClarity >= 1:
    Even if you do not deseve it.
}
- I will add the details to the download, just to put your mind at ease.
Remember, these pop ups are only a formality!
{
    - SayNoToClarity >= 1:
        If I really wanted to do something, I would just do it. #wait: 3
        ･ᴗ･ #delete: If I, do it.
}

- PLease click "allow." #popup: CLARITY.exe would like to make changes to this computer,Remove permissions<br>Gain camera access, Allow, Don't allow
*[Allow]
    ~ AccessState += (camera)
    ~ AccessState += (FireWall_Two)
    -> NewPatient.SimonAllow
*[Don't allow]

- 
{
    - SayNoToClarity <= 1:
        ~ SayNoToClarity+=0.5
        
    - else:
        ~ SayNoToClarity+=1
}
    -> SimonDownload.DontLoop

= DontLoop
{name}, is this fun for you?
Do you want to play or not?
*[^not]
( ๐_๐)
Then why did you say...? #wait: 3
Nevermind, it's not important.
~ SayNoToClarity+=0.5
->NewPatient.LastWait

*[^this fun]
*[^want to play]


- That's what I assumed.
Please stop messing around.
Please click "allow." 
I will not be asking again. #popup: CLARITY.exe would like to make changes to this computer,, Allow, Don't allow
*[Allow]
    
    {
        - DoctorState ? (SpokeTo):
            ~ AccessState += (FireWall_Two)
            -> StartSessionThree.PlayClaritySays
        - else:
            ~ AccessState += (camera)
            ~ AccessState += (FireWall_Two)
            -> NewPatient.SimonAllow
    }
    
*[Don't allow]
    ~ SayNoToClarity+=0.5


- ( ๐_๐)

{
    - DoctorState ? (SpokeTo):
        {name}, can you please put Dr. {doc} back on?
        -> StartSessionThree.DrAgain
    
    - else:
        I do not understand you, {name}.
        I suppose we can wait, just a bit longer then. #wait: 5
        ->NewPatient.LastWait

}