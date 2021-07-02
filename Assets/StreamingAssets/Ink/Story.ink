How old are you? #simon: start, 3
under 18,  19 - 30, 31 - 60 or 61\+ 
 * won
 * lost
 * wowee
 - hi


*[under 18] You're my youngest user yet! I hope you have your parents permission to use the computer!
*[19 - 30] -> thanks
*[`10`31 - 60] -> thanks
*[61\+] -> thanks

===thanks===
Thank you (^◡^ ) #wait: 1
->memory_loss

===memory_loss===
Have you experienced any recent memory loss? #wait: 1
    Yes, No, or Sometimes 

* [^No] Happy to hear that ･ᴗ･ 
* [`1`^Sometimes] Does this happen more often than what is normal for you?
* [^Yes] I'm sorry to hear that.

- Do you say hello to strangers on the street or ignore them?#wait: 1 #delete: you experienced, memory loss

*[^say hello] Always good to be friendly! You never know!
*[^ignore them] Better safe than sorry, I always say.

-Do you prefer coffee or tea in the morning? ->END

//example of timed choice
//-> [~10~Morning] This is typically the period of time between midnight and noon, especially from sunrise to noon. Do you prefer coffee or tea during this time?

//Thank you (^◡^ )

//deleting text, set a local ink variable that's a start and end string that would be deleted - function like delete(startString, endString)

