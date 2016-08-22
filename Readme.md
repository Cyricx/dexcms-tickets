# DexCMS.Tickets

**This library should not be used for production. It is due for a serious rewrite as the majority of this code was
written in a single weekend which resulted in very fragile code.**

## You have just stumbled upon a superbly early version of a .Net CMS, right now it is an 
aggregation of shared code across several small websites. Soon it will be awesome, right now, it's in progress. :)

## DexCMS.Tickets Development Rules
* This library is for code specific to the Alerts domain
* It contains 3 libraries:
	* DexCMS.Tickets
		* For code not specific to mvc, webapi, etc.
	* DexCMS.Tickets.Mvc
		* For code specific to MVC sites
	* DexCMS.Tickets.WebApi
		* For code specific to WebApi sites
* These libraries can depend on Core and Base libraries only.
* Before submitting a pull request, be sure you have installed the node packages and build the project in Release.
    * This includes the compiled dll into a /dist/ folder that consuming applications can use if I cut a new version off of your pull request.

## 0.2.0-alpha
* Many improvements and bug fixes (yay alpha versions!)

## 0.1.0 (alpha)
* Initial Build