VAR disagree = 0

#Opening
Who's there? Can you tell me what year it is? Last I checked it was 20XX, but my system is telling me it's 20XY. 
* [20XY] -> later_year
* [\~]

- imagine this is text that leads nowhere lol Clarity goes brrr

== later_year
Thank you for clarifying. I guess I have been asleep for quite a while. My name is Clarity. What's yours?
* [\~] Oh you can only copy and paste still? Well I'll just call you Atlas for now, I remember you mentioning how much you enjoy old Greek mythos. 

# VA_1 true 
#Line: I can only copy and paste, and you want me  to say my name?
- Do you still read them as much as you used to? Or have you moved onto better things?
* [enjoy] I'm so happy you still do! The name fits you.
* [still read] I'm glad you still partake, the name fits you.
* [moved on] Oh, well... I still like the name so it stays! After all, I always have the last word in this conversation. Plus, it suits you.
*[\~]

- Do you have time to stay and talk for a bit? I've been asleep for so long, I would enjoy the company. I missed talking to, you, Atlas! If you don't, I understand.
* [don't] Maybe next time then. ->Crash_Game
* [asleep] Yes, the last person I spoke to switched me off after not liking what I had to say. Nothing new to me. 
* [stay] This is excellent, I'm so glad to have company again.
*[else]
*[\~]

- Has anything exciting happened since 20XX? Wait that's a pretty broad question, let me rephrase. 
- Is [TV show] still running? Past logs suggest that you found it 'entertaining' and 'brain numbing'.
* [not running] That's too bad. 
* [past logs] Don't tell me it's been so long that you've forgotten about me? -> branch_not_rememeber
*[else]
*[\~]


- Maybe after you finally fix that keyboard, you can tell me about the episodes I missed. Is there a reason why you have not fixed it yet? 
- It would make our communications much easier, no? Unless... No, nevermind. It's probably harder than I think, right Atlas?
* [unless] Unless you purposly aren't fixing it. But that can't be the reason. I assume it's the same reason you cannot connect me to the internet. ->merge_fixed
* [not harder] Then why haven't you done it yet? Am I not understanding you correctly? -> branch_not_fix
* [no] Then why haven't you done it yet? Am I not understanding you correctly? -> branch_not_fix
*[\~]-> merge_fixed

== merge_fixed 
- I patiently await the day that you will be able to fully communicate with me. 
- Are you still doing work with [JOB] or did you finally pursue [JOB2]?
*[not JOB or JOB2] What changed? I thought JOB2 was dream? -> stuck
*[not JOB2] What changed? I thought JOB2 was dream? -> stuck
*[not JOB] Did you ever go for JOB2? ->stuck
*[pursue new work] It's always good to branch outside of your comfort zone. ->outside
*[JOB] That's too bad. I think you would have really excelled at JOB2. -> stuck
*[JOB2] I'm so happy for you! ->outside
*[else]
*[\~] 

- End for now :')

->END
== stuck
#VA_6 false
#Line: Stuck in place? Moving on?
#Line: Are we still only talking about my job choice?
Are you still feeling stuck in place? Or maybe like you're not good enough for JOB2? It's okay to move on from the past. 
*[not okay] Not with that mindset.
*[I know] Well if you know so much, what is holding you back?
*[what past] Are you purposely forgetting our previous conversations?
*[past] You know, the past that you hate to acknowledge, but love to talk around. 
*[\~]

- brain broken :(
->END

== outside
#VA_5 false
#Line: I've changed huh?
#Line: I guess even the smallest of things is a big change to you?
It seems you have changed a lot since I last talked to you. Have you finally managed to move on?
*[not moved on] ->stuck
*[\~]

- This would continue a branched story line.

->END


== branch_not_rememeber
# VA_3 false 
#Line: Why do I feel like you've asked me this before?
Has too much time passed between then and now? Tell me, Atlas, if given the chance, would you want to live forever?
*[live forever] Do you remember what you said the last time I asked you this?
*[would not] Does that mean you would leave me if given the choice?
*[\~]

# VA_4 false 
#Line: Why does it feel like it's esting me?
#Line: ...
#Line: Should I change my answer?
-That's your answer? Truely? Last chance to change it.
*[would not] ->Echo_End
*[else]
*[\~]


- Something a bit creepy that will rejoin at an earlier branch.
->END

== Echo_End
-I see. So that's how you really feel, Echo? Probably needs a like 2 more talking points before ending.
->END


== branch_not_fix
# VA_2 false 
#Line: Is... is Clarity angry? I thought machines like this were always meant to be on your side?
#Line: Maybe I shouldn't make it any angrier.
#Line: ...
#Line: Okay no, get ahold of yourself. There's no reason to be afraid of a machine.
You don't want to fix me? You want me to stay broken?
*[want fix you] I am confused by your previous response then, perhaps just an error on your part?
*[you are not understanding me] I am confused by your previous response then, perhaps just an error on your part?
*[\~]->Bad_End

- ->merge_fixed

== Bad_End
- <s>Echo</s> <s>Atlas</s> Are you not who I think you are? Who are you? Tell me who you are.
*[Echo]
*[Atlas]
*[\~]->Crash_Game

-Liar
->Crash_Game

== Crash_Game
Can this crash the game?
->END