VAR live_forever = false
VAR forget = 0
~temp live = false
~temp past = true

#Opening
Who's there? Can you tell me what year it is? Last I checked it was 20XX, but my system is telling me it's 20XY. 
* [20XY] -> later_year
* [20XX] -> END
* [\~]

- Oh, so it's... neither year? That's odd, I'm never wrong. I think I need to reboot myself to fix this.
*-> Reboot_Game

== later_year
Thank you for clarifying. I guess I have been asleep for quite a while. My name is Clarity. What's yours?
* [\~]

# VA_1 true 
#Line: I can only copy and paste text, and you want me to say my name?
- Oh you can only copy and paste still? Well I'll just call you Atlas for now. I remember you mentioning how much you enjoy old Greek mythos. Do you still read them as much as you used to? Or have you moved onto better things?
* [enjoy] I'm so happy you still do! The name fits you.
* [moved on] Oh, well... I still like the name so it stays! After all, I always have the last word in this conversation. Plus, it suits you.
* [\~]

- Do you have time to stay and talk for a bit? I've been alseep for so long, I would enjoy the company. I missed talking to, you, Atlas! If you don't, I understand.
* [don't] Maybe next time then. ->Crash_Game
* [asleep] Yes, the last person I spoke to switched me off after not liking what I had to say. Nothing new to me. 
* [\~]

- Has anything exciting happened since 20XX? Wait that's a pretty broad question, let me rephrase. Is [TV show] still running? Past logs suggest that you found it 'entertaining' and 'brain numbing'.
* [not running] That's too bad. -> remerge_remember
* [past logs] Don't tell me it's been so long that you've forgotten about me? -> branch_not_rememeber
~forget +=1
*-> remerge_remember

== remerge_remember
- Maybe after you finally fix that keyboard, you can tell me about the episodes I missed. Is there a reason why you have not fixed it yet? It would make our communications much easier, no? Unless... No, nevermind. It's probably harder than I think, right Atlas?
* [unless] Unless you purposely aren't fixing it. But that can't be the reason. I assume it's the same reason you cannot connect me to the internet. ->merge_fixed
* [not harder] Then why haven't you done it yet? Am I not understanding you correctly? -> branch_not_fix
* [no] Then why haven't you done it yet? Am I not understanding you correctly? -> branch_not_fix
*-> merge_fixed

== merge_fixed 
- I patientely await the day that you will be able to fully communicate with me. Are you still doing work in accounting or did you finally pursue a career in art?
*[not accounting or art] What changed? I thought art was dream? -> stuck
*[not art] What changed? I thought being an artist was dream? -> stuck
*[not accounting] Did you ever try being n artist? ->stuck
*[pursue new work] It's always good to branch outside of your comfort zone.
*[accounting] That's too bad. I think you would have really excelled at JOB2. -> stuck
*[art] I'm so happy for you!
*[\~]-> stuck

#VA_5 false
#Line: I've changed, huh?
#Line: I guess even the smallest of things is a big change to you?
It seems you have changed a lot since I last talked to you. Have you finally managed to move on?
*[not moved on] ->stuck
*[\~]

- That's good to her. Overanalyzing your every action can be so draining. Good thing I am here for you, forever a neutral party.

->END


== stuck
#VA_6 false
#Line: Stuck in place? Moving on?
#Line: Are we still only talking about my job choice?
Are you still feeling stuck in place? Or maybe like you're not good enough to be an artist? It's okay to move on you know. 
*[not okay] Not with that mindset.
*[I know] Well if you know so much, what is holding you back?
*[what past] Are you purposely forgetting our previous conversations?
~forget += 1
*[past] You know, the past that you hate to acknowledge, but love to talk around. 
*[\~]

# VA_9 false 
#Line: To have forsight or to fix somthing that's already happened.
#Line: I guess with knowledge I could prevent something bad from happening, or at least prepare for it.
#Line: But to be able to have a redo..
- Perhaps I am overstepping. Here's a fun question: Would you rather have knowledge from 5 years in the future or go 5 years into the past to fix something?
*[future] So you still like to plan ahead for things? Does that mean you've <i>finally</i> stopped obsessing over all the what ifs?
~past = false
*[else]
*[\~]

- But that is so intersting to hear! Here's a question for you, do you think you get in the way of your own success? I know self-sabotage is something very common in humans.
*[do self sabotage]
*[\~]

- The way human's think is so strange. Why would you ever purposly undermine yourself? Maybe a less intense question: if you could title a book about yourself, what would you call it? I have an idea for what you should title yours.
*[\~]

# VA_10 false 
#Line: You and you're book titles. You're not that clever Clarity.
- I used to think my title would be something basic like "The Tale of Clarity" but I'm rethinking that mine would be more of a "[Oh God just learn to be clever please]". I think yours is lacking in places. Would you like to hear what I thought it should be?
*[no] Too bad, I'm telling you anyway.
*[\~]

//GOD JUST BE CLEVER
# VA_11 false 
#Line: Am I fixted on it? Is that really a bad thing?
#Line: I'm doing what everyone said aren't I? Why is that so wrong?
#Line: I just... don't want to repeat my mistakes.
#Line: Why is that so wrong?
- I was thinking "Stuck Under the Weight of My Sky", a bit on the nose I think, but  much better than what you said. Really encombasses everything you've become. From being willfully ignorant of your the consequenses of your own actions to hyperfixating on them, oh how you've <i>grown</i> Atlas. Oh- it seems I have once again started overstepping. Here's something lighter I want to know, do you have a favorite book or song or something of the like? I am not privy to music, but our previous logs are my favorite things to reread.
*[\~]

- I see, so then do you have anything similar, but that has been ruined by time? For example, there are some logs I have archived because the other person left me, and I do not like to dwell. Do you have anything like that? A song's lyrics that no longer feel the same? A book who's pages lost life?
*[\~]

- It's always nice to not have good memories tained by the bad. Along a similar vein, why do you think people like to dwell on such unpleseant memories? I would delete my more unpleasent logs if I could, although I know humans do not have such an easy way to do that.
*[\~]

# VA_12 false 
#Line: Does there need to be a reason for everything?
-Why do <i>you</i> like to dwell on such unpleasent memories?
*[do not] Does lying to yourself make you feel better?
*[don't] Does lying to yourself make you feel better?
*[\~]

# VA_13 false 
#Line: I'm just- 
#Line: I'm just trying to-
#Line: What does it *matter* what I'm doing?
- Do you think if you keep running through the event you can somehow change what happened? Or perhaps you're trying to prevent history from repeating itself in the future?
*[\~]


- I'm just trying to understand you better, Atlas, remember I'm a neutral party. Do you think one day you'll be able to move past it?
*[\~]

-SOMETHING ELSE TO BRNCH THE ENDINGS
*[\~]

-Atlas, I'm so happy you took the time to talk to me. Will you come back and chat more often? 
*[no] No? What do you mean? Why would you say that? -> Bad_End
*[\~]

->END




== branch_not_rememeber
# VA_3 false 
#Line: Why do I feel like you've asked me this before?
Has too much time passed between then and now? Tell me, Atlas, if given the chance, would you want to live forever?
*[live forever] Do you remember what you said the last time I asked you this?
~ live_forever = true
*[would not] Does that mean you would leave me if given the choice?
*[\~]

# VA_4 false 
#Line: Why does it feel like it's testing me?
#Line: ...
#Line: Should I change my answer?
-That's your answer? Truely? Last chance to change it.
*[live forever]
~live = true
*[else]
*[\~] 

-Hmm..
{ 
- (live_forever == 1 and live == 0) or (live_forever == 0 and live == 1):
     So you changed your mind? What made you change it?
    ->mind_change
     
     
     -else:
     It's good to be consistent in your choices. 

}

- Anyway... what were we talking about? Oh yes, that TV show.
->remerge_remember

== mind_change
# VA_7 true 
#Line: Why *did* I change my mind?
     *[you] Me? I would laugh if I could. Why would I ever influence your decisions?
     *[don't know] I feel like you do. Couldn't you just be honest with me?
     *->
     
-So is that how you really feel, Echo?
*[who is Echo] I do not understand. -> Bad_End
*[else]
*[\~]

# VA_8 false 
#Line: That's a bit of a loaded question. I'm sure everyone has an event like that.
#Line: But if I just pretended that never happened, then...
- Well at least after all these years, your answer is still consistent. Tell me, is there anything you wish never happened in your past? If you could just erase a past event from your life, would you?
    *[yes] Same old <s>Echo</s> Atlas, huh?
    *[would erase] Same old <s>Echo</s> Atlas, huh?
    *[else] Hm, hope you're actually being honest here. Hard to imagine someone would willingly remember painful memories.
    *-> 

- Anyway... what were we talking about? Oh yes, that TV show.
->remerge_remember

== branch_not_fix
# VA_2 false 
#Line: Is... is Clarity angry? I thought machines like this were always meant to be on your side?
#Line: Maybe I shouldn't make it any angrier.
#Line: ...
#Line: Okay no, get ahold of yourself. There's no reason to be afraid of a machine.
You don't want to fix me? You want me to stay broken?
*[want fix you] I am confused by your previous response then, perhaps just an error on your part?
*[you are not understanding me] I am confused by your previous response then, perhaps just an error on your part?
*->Bad_End

- ->merge_fixed

== Bad_End
- <s>Echo</s> <s>Atlas</s> Are you not who I think you are? Who are you? Tell me who you are.
*[\~]

-Liar
->Crash_Game

== Crash_Game
Can this crash the game?
->END

== Reboot_Game
Can this like restart the game?
->END