# DogmaMix.Core
This project provides additional general-purpose functionality, utility classes,
and extension methods for the .NET platform.

* **DogmaMix.Core** builds on the .NET Standard libraries. 
  Its types do not have any dependencies other than the system assemblies.
* **DogmaMix.Core.Dependents** builds on popular libraries for the .NET platform,
  such as the Microsoft Test Framework (MSTest). 
  Its types have dependencies on the said libraries, which also need to be referenced from any consuming projects.
* **DogmaMix.Core.Tests** contains unit tests and integration tests for the above.

### Why was this project created?
This project was developed to address common requirements for additional general-purpose functionality in .NET, 
as elicited from anecdotal experience and popular questions on Stack Overflow. 
By releasing a production-grade implementation – XML-documented and unit-tested – under a permissive 
open-source license, it is aspired that this code will be reused liberally wherever required.

### Can I use this project in my software?
This project is released under the [MIT License](https://opensource.org/licenses/MIT), meaning that you can use it 
wherever you want – including for proprietary software – as long as you provide attribution. 
However, keep in mind that this project’s public API is still not stable –
future updates may introduce breaking changes to its definitions. 
Rather than downloading the entire project, we encourage you to copy any parts you find useful 
directly into your code and adapt them per your requirements.

### How do I provide attribution?
Pasting the URL of the [GitHub repository](https://github.com/Douglas-Williams/DogmaMix.Core) 
as a comment in your source code is sufficient. 
You do not have to include the full MIT License, nor provide public attribution in your end-product. 
If you’re copying types or members whose XML documentation contains references to other sources, 
such as Stack Overflow, you should preserve those as well.
Aside from providing attribution, such links maintain traceability for the origin of your code.

### Do we really need another utilities library?
You probably don’t. This project is a compilation of the functionality that the author personally found useful 
or interesting during their own development, and will likely only share a small overlap with your needs.
For this reason, we encourage you to pick out whichever parts you find useful.