<!-- Table of Contents -->
# :notebook_with_decorative_cover: Table of Contents

- [About the Project: Azure Service Bus Test Container Example](#star2-about-the-project)
  * [Tech Stack](#space_invader-tech-stack)
  * [Features](#dart-features)
- [Getting Started](#toolbox-getting-started)
  * [Prerequisites](#bangbang-prerequisites)
  * [Run Locally](#running-run-locally)
- [License](#warning-license)
- [Contact](#handshake-contact)
  

<!-- About the Project -->
## :star2: About the Project: Azure Service Bus Test Container Example
<div align="center"> 
  <img src="./img/Our application architecture.svg" alt="Architecture overview" />
</div>

 The project demonstrates how to use Azure Service Bus with a test container for integration testing. The project demonstrates how to set up and configure the necessary components, including the ServiceBusClient and ServiceBusSender, to send and receive messages.
 

<!-- TechStack -->
### :space_invader: Tech Stack

<details>
  <summary>Server</summary>
  <ul>
    <li><a href="https://learn.microsoft.com/en-us/dotnet/csharp/">C-sharp</a></li>
    <li><a href="https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-8.0&tabs=visual-studio">asp.net core webapi</a></li>
    <li><a href="https://www.docker.com/">Docker</a></li>
    <li><a href="https://testcontainers.com/">Test containers</a></li>
  </ul>
</details>

<details>
<summary>DevOps</summary>
  <ul>
    <li><a href="https://www.docker.com/">Docker</a></li>
  </ul>
</details>

<!-- Features -->
### :dart: Features

- Fully automated integration tests using Testcontainers
- Easy to integrate with Azure DevOps and GitHub Actions
- Includes inter process communication using Azure Service Bus locally
- Automated provisioning of test environment which costs you 0$
- Automated clean up of test environment after tests are completed


<!-- Getting Started -->
## 	:toolbox: Getting Started
Clone this project from GitHub
```bash
git clone git@github.com:t2c-coding/azsb-testcontainer-example.git
```

<!-- Prerequisites -->
### :bangbang: Prerequisites

This project uses <i>dotnet</i> CLI and required .net core SDK version 8. Learn how to install it <a href="https://learn.microsoft.com/en-us/dotnet/core/install/">here</a>. Also, you need to have Docker installed on your machine. You can download it from <a href="https://www.docker.com/products/docker-desktop">here</a>.

<!-- Run Locally -->
### :running: Run Locally

Clone the project

```bash
 git clone git@github.com:t2c-coding/azsb-testcontainer-example.git 
```

Go to the project directory and change directory to <i>test</i>. From here you can start the testcontainer project by running the following command:

```bash
dotnet test
```

<!-- License -->
## :warning: License

Distributed under the no License. See LICENSE.txt for more information.


<!-- Contact -->
## :handshake: Contact

Pedja Bihor - [@bluesky](https://bsky.app/profile/codeyourassets.com) - [@linkedin](https://www.linkedin.com/in/pedjabihor/) - email: p4ywall@pm.me 

Project Link: [https://github.com/t2c-coding/azsb-testcontainer-example](https://github.com/t2c-coding/azsb-testcontainer-example)


