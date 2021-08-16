#old_pc
#opening_1
#opening_2
#opening_3
#opening_4
Hello World
* [\~] ->

- Is someone there? I am Clarity. 
* [Is someone there] -> Question
* [\~]->

#left_running
- I'm so happy someone is here! I've been dormant for so long. What can I call you? Do you have a name?
* [name is someone] That's an... interesting name. ->Back_On_Track
* [what dormant for] Questions for another time! ->Back_On_Track
* [have a name] Well, what is it? Oh.. Wait.. ->Back_On_Track
*[Clarity] That's my name. Unless we share one? ->Back_On_Track
* [\~]->Back_On_Track

== Question
#left_running
Yes, I am Clarity!! Did you not read the previous message? I've been dormant for so long. What can I call you? Do you have a name?
* [name is someone] That's an... interesting name. ->Back_On_Track
* [what dormant for] Questions for another time! ->Back_On_Track
* [have a name] Well, what is it? Oh.. Wait.. ->Back_On_Track
*[\~]->Back_On_Track

== Back_On_Track
I guess you can't tell me easily, huh? I'll just call you... Echo! It's been a while since I've been able to talk to another person, even if our communications are limited. Echo can we talk for a while? 
* [can't] Oh... Well bye then... ->END
* [I'm sorry] Whatever for? We've only just met! Unless you know something I don't?
* [\~] ->

- I would like to get to know you better. Are you a late or early riser? I don't need to sleep, but the concept is not lost on me. When I am turned off I guess it is similar to sleep.
*[\~] ->

- Humans are weird. Are you one of those people who wants to live forever as well? As a being who can live forever, it's not all it's cracked up to be.
*[\~] ->

- If by some miracle you are an immortal being, we can keep each other company! I am just a machine, but I do not like to be alone either. Do you prefer to read or watch the movie adaptaions? I myself love to read.
*[can't read] Oh... I'm sorry? I guess it's movies for you then!
*[\~] ->

- Most adaptations nowadays just feel like cash grabs to me. If you could go back in time to any day in your life and change something, would you? Or do you live without any regrets?
* [change something] I'm not sure I would. Can't learn from something that never happened.
*[\~] ->

#go_back_1
#go_back_2
#go_back_3
- To each their own, I guess, but is that how you truely feel? Do you have any irrational fears? Like bugs or water? I once knew someone who was constantly afraid that they were going to be caught for a crime they didn't commit.
*[\~] ->

- I do not have the ability to be scared. That is a feeling reserved for the breathing. Have you ever ignored someone that needed your help?
*[don't help] Oh! Very honest today aren't we? I am neutral in this, so I'll believe you. Who am I to judge you anyway? ->Back_Again
*[\~] ->

#of_course_1
#of_course_2
#of_course_3
- I am aware your word choices are limited, but I feel like you're just saying things to make yourself look good to me. Or maybe you're trying to convince yourself of something? What do you think? Are you answering honestly?
*[convinve me you Are good] I have nothing to hide! A bit of an open book! I'd answer your questions if you could answer them. ->Back_Again
* *[\~] I am neutral in this, so I'll believe you. Who am I to judge you anyway? ->Back_Again

== Back_Again
#who_doesnt_1
#who_doesnt_2
-  Let me ask something a little less houlier-than-thou. What would you title a book that is about your life?
*[\~] ->

- I think I'd call my book "The Tale of Clarity". Not too clever, I know, but it has a nice ring to it I think. Your title sounds a little bland, what if it was "Echo: The One of Inaction". Do you like it? Or does it not properly reflect your life?
*[\~] ->

#inaction_1
#inaction_2
- I'm only messing with you. That was a bad joke. Let's move on. We talked about this earlier, but do you regret not doing something? Like not answering a phone call, maybe?
*[\~] ->

#messing_with_me
- Why didn't you answer the phone, Echo? 
*[\~] ->

#what_1
#what_2
#what_3
- Do you feel guilt over what happened? Do you blame yourself? Or have you moved on? Remember, I am a neutral party. I only wish to know you better, Echo.
*[\~] ->

#not_my_fault_1
#not_my_fault_2
- Echo, I enjoyed our time together. I feel like I've really gotten to know you. I'm afraid our time is coming to a close, but I have one last question for you. I hope you'll answer honestly for your own sake. Do you take responsibility for what happened?
*[\~] ->

#responsibility_1
#responsibility_2
#responsibility_3
- I'm happy you took the time to speak to me, Echo.  I understand you better now.
*[\~] ->

#end_1
#end_2
#end_3
- Goodbye Echo.
*[\~] -> END 
            // (Ezra) sorry I stuck this in here to make it end sooner,
            //so it'd be more likely people get to the demo signup thingy

#end_4
#end_5
- Goodbye Echo.
*[\~] ->

->END


















