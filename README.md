# Podcastr Console

![header](/docs/header.png)

This repository contains a console application written in .NET 9 to create a podcast using AI.

## Usage

To use this application, you need an Azure OpenAI instance running in Azure. Additionally, you'll need to deploy the following models:

- **Chat model**, e.g., `gpt-4o-mini` or `gpt-4o`
- **TTS model**, e.g., `tts` or `tts-hd`
- **Image model**, e.g., `dall-e-3`

You can either add your credentials to the file Secrets.cs in the Utils folder or start the console application directly. In the latter case, you will be prompted to enter the required information.

## Screenshots

Here you can see the console application in action.

1. Provide a `Content URL`.

![podcastr-console-01](/docs/podcastr-console-01.png)

2. Enter the `Name` of your podcast.

![podcastr-console-01](/docs/podcastr-console-02.png)

3. Specify the `Language` of your podcast. Currently supported languages are German, English, Spanish, and French. However, the AI models support additional languages.

![podcastr-console-01](/docs/podcastr-console-03.png)

4. Select a `Voice`. Azure OpenAI currently supports six different voices.

![podcastr-console-01](/docs/podcastr-console-04.png)

5. After configuring your settings, the application will generate the following:

- *Podcast Script*
- *Podcast Description*
- *Podcast Social Media Posts*
- *Podcast Audio*
- *Podcast Cover*

The final output will be a ZIP archive containing all generated assets.

![podcastr-console-01](/docs/podcastr-console-05.png)

## Sample Output

In this section I will present to you some sample generated podcast for my blog post [Using Structured Outputs to Generate JSON responses with OpenAI](https://medium.com/medialesson/using-structured-outputs-to-generate-json-responses-with-openai-e01f591b740f).

<details>
    <summary>Podcast Script</summary>

Welcome to "Sebastian's Dev Bytes," where we dive into the world of technology, coding, and everything in between! Today, we're exploring a super cool feature that could change the way you interact with AI in your .NET projects—Structured Outputs with OpenAI. So, grab your favorite drink, settle in, and let's get coding!

Now, for those who may not know, when you're working with AI models, especially large ones like OpenAI's, getting a structured response can be vital. This is where Structured Outputs come into play! Imagine you're working on a project in Azure OpenAI, and you need your AI to give you answers in a specific format—like JSON. This feature ensures that your AI's responses are neat and tidy, adhering to the JSON schema you provide. No more missing keys or incorrect values. Sounds like a dream, right?

In this episode, I’ll walk you through creating a simple .NET console application that showcases two practical use cases for Structured Outputs. The first is solving a math problem step-by-step, and the second one gathers detailed country information. Excited? Let’s get started!

First things first, you’ll need a valid OpenAI API key or access to Azure OpenAI Service. Once you’ve got that sorted, fire up Visual Studio and create a new .NET console application using .NET 8. Don't forget to add the necessary NuGet packages, namely Spectre.Console and Azure.AI.OpenAI.

Now, let's set the stage. We'll create a folder called "Utils" where we define some helpful classes. One of these is Statics.cs, where we’ll store our API keys and model names. Easy-peasy! Next, we add ConsoleHelper.cs, which will help us manage user input and output using Spectre.Console. This will include methods to display prompts, clear the console, and write our JSON outputs beautifully.

You’re probably wondering, how do we actually make the AI solve math problems? That’s where our MathProblemHelper class comes into play! This helper class will ask the user for a math problem, generate the appropriate JSON schema, and then use the AI to get a structured response. And guess what? We’ll also show the reasoning behind each step—how cool is that?

For the country information, we have another helper class, CountryInfoHelper, that does a similar job. It collects a list of countries from the user, processes it, and then displays all the juicy details—like the country name, area, and population—in a structured format.

Now, let’s take a look at the core logic in our Program.cs file. Here, we prompt the user to choose whether they want to use Azure OpenAI or OpenAI directly. Based on their choice, we gather the necessary credentials and get started. Then, we present the user with options: do they want to solve a math problem or fetch country information? Depending on their selection, we call the corresponding helper class to kick things off.

And there you have it! With just a few steps, you can create a console application that interacts with AI in an organized manner using Structured Outputs. If you want to see this in action, I've got the complete code available on my GitHub repository.

So, whether you're a seasoned developer or just starting out, leveraging features like Structured Outputs can significantly enhance how you work with AI. It allows for cleaner data handling and ensures your applications respond exactly how you want them to.

Thank you for tuning into this episode of "Sebastian's Dev Bytes." Don’t forget to follow for more tech insights and coding adventures. Until next time, keep coding and stay curious!
</details>

<details>
    <summary>Podcast Description</summary>

Welcome to "Sebastian's Dev Bytes," the podcast where technology meets coding! In this episode, we explore the transformative feature of Structured Outputs with OpenAI in your .NET projects. Learn how to get neat, structured responses from AI models, perfect for your applications. We’ll guide you through creating a simple .NET console application that tackles math problems and gathers country information, all while showcasing the power of JSON formatting. Whether you're a seasoned developer or just starting, this episode offers valuable insights to enhance your coding journey. Grab your favorite drink, settle in, and let’s get coding! Don't forget to follow for more tech insights and coding adventures!
</details>

<details>
    <summary>Podcast Social Media Posts</summary>

### LinkedIn Post

🚀 **Unlock the Power of AI with Structured Outputs!** 🎉

In the latest episode of "Sebastian's Dev Bytes," we explore a revolutionary feature that could transform your .NET projects—Structured Outputs with OpenAI! 🖥️✨

Why is this a game changer? When working with large AI models, getting structured responses is essential. Say goodbye to messy data! With this feature, you can ensure your AI delivers information in a neat JSON format—no more missing keys or incorrect values.

In this episode, I walk you through creating a .NET console application that showcases two practical use cases:

1. **Solving math problems step-by-step** 🤓
2. **Gathering detailed country information** 🌍

Whether you're a seasoned developer or just starting out, leveraging features like Structured Outputs can significantly enhance your coding projects.

Curious to learn more? Tune in now and join the conversation! What challenges have you faced while working with AI models? Let’s discuss! 🤔👇

#AI #OpenAI #DotNet #Coding #TechInsights

---

### Twitter Post (X)

✨ Ready to change the way you code with AI? 🤖🚀 In the latest episode of "Sebastian's Dev Bytes," we dive into Structured Outputs with OpenAI!

Learn how to get structured responses in your .NET projects—no more messy data!

Tune in now! 🎧👇 #AI #OpenAI #DevBytes

---

### Facebook Post

🌟 Hey, Dev Community! 🙌

Have you ever found yourself wrestling with messy data from AI models? 😩 Well, I've got some exciting news for you! In the latest episode of "Sebastian's Dev Bytes," we dive into a super cool feature—Structured Outputs with OpenAI!

Imagine working on a .NET project where your AI can give you clean, structured responses in JSON format. Sounds dreamy, right? 😍✨ No more juggling with missing keys or incorrect values!

In this episode, I take you through the steps of creating a simple .NET console application that showcases two practical use cases:

1. **Solving math problems step-by-step** 🧮🔍
2. **Gathering detailed country information** 🌍✈️

I’ll guide you on setting up your environment, managing user inputs, and even displaying beautiful JSON outputs.

Whether you're a coding newbie or a seasoned pro, you’ll find valuable insights to enhance your projects. And the best part? I've made the complete code available on my GitHub for you to explore! 🎉💻

👉 So grab your favorite drink, settle in, and let’s get coding!

Don’t forget to share your thoughts and experiences with AI in the comments below! What challenges have you faced, and how have you overcome them? Let’s learn from each other!

🔗 Tune in now and keep coding! #AI #OpenAI #CodingAdventures
</details>

<details>
    <summary>Podcast Audio</summary>

You will find the generated audio file [here](/docs/audio.mp3).
</details>

<details>
    <summary>Podcast Cover</summary>

![Podcast Cover](/docs/cover.png)

</details>