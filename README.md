# AT93C56

Projekt testowy. Nauka GitHub-a.
***
## **Headers**

# H1
## H2
### H3
#### H4
##### H5
###### H6


***
## **Bold**

**BOLD**

***

## **Italic**

*Italic*

***

## **Bold + Italic**

***Bold & Italic***

***

## **Strikethrough**

~~Strikethrough~~



***
 
## **js** code
```js
function areEqual(a, b) {
	if (a === b) {
		return false;
	}
}
```
***

## **c** code
```c
public BluetoothLEDeviceInfoModel(DeviceInformation deviceInformation)
{
    this.Id = deviceInformation.Id;
    this.Name = deviceInformation.Name;
    this.IsEnabled = deviceInformation.IsEnabled;
    this.IsPaired = deviceInformation.Pairing.IsPaired;
}
```
***
 
## **JavaScript** code
```JavaScript
var arr = [ "one", "two", "three", "four", "five" ];
var obj = { one: 1, two: 2, three: 3, four: 4, five: 5 };
 
jQuery.each( arr, function( i, val ) {
  $( "#" + val ).text( "Mine is " + val + "." );
 
  // Will stop running after "three"
  return ( val !== "three" );
});
 
jQuery.each( obj, function( i, val ) {
  $( "#" + i ).append( document.createTextNode( " - " + val ) );
});
```

***
## **csharp** code

```csharp
    using (var client = new HttpClient())
    {
        // Set address of server
        client.BaseAddress = new Uri("http://localhost:9000/");

        // Clears the RequestHeaders before setting a new one
        client.DefaultRequestHeaders.Accept.Clear();
        // Set RequestHeaders to accept only JSON
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
```

***

## **Links**

[I'm an inline-style link](https://www.google.com)

[I'm an inline-style link with title](https://www.google.com "Google's Homepage")

[I'm a reference-style link][Arbitrary case-insensitive reference text]

[I'm a relative reference to a repository file](../blob/master/LICENSE)

[You can use numbers for reference-style link definitions][1]

Or leave it empty and use the [link text itself].

URLs and URLs in angle brackets will automatically get turned into links. 
http://www.example.com or <http://www.example.com> and sometimes 
example.com (but not on Github, for example).

Some text to show that the reference links can follow later.

[arbitrary case-insensitive reference text]: https://www.mozilla.org
[1]: http://slashdot.org
[link text itself]: http://www.reddit.com


***
## **Images**
Markdown: ![Benjamin Bannekat](http://octodex.github.com/images/foundingfather_v2.png).















































***
## **Blockquotes**

> "In a few moments he was barefoot, his stockings folded in his pockets and his
  canvas shoes dangling by their knotted laces over his shoulders and, picking a
  pointed salt-eaten stick out of the jetsam among the rocks, he clambered down
  the slope of the breakwater."

***
## **Lists**

* Milk
* Eggs
* Salmon
* Butter

1. Crack three eggs over a bowl
2. Pour a gallon of milk into the bowl
3. Rub the salmon vigorously with butter
4. Drop the salmon into the egg-milk bowl


* Tintin
  * A reporter
  * Has poofy orange hair
  * Friends with the world's most awesome dog
* Haddock
  * A sea captain
  * Has a fantastic beard
  * Loves whiskey
    * Possibly also scotch?

***

1. Crack three eggs over a bowl.

	Now, you're going to want to crack the eggs in such a way that you don't make a mess.

	If you _do_ make a mess, use a towel to clean it up!

2. Pour a gallon of milk into the bowl.

	Basically, take the same guidance as above: don't be messy, but if you are, clean it up!

3. Rub the salmon vigorously with butter.

	By "vigorous," we mean a strictly vertical motion. Julia Child once quipped:
   > Up and down and all around, that's how butter on salmon goes.
4. Drop the salmon into the egg-milk bowl.

   Here are some techniques on salmon-dropping:
    * Make sure no trout or children are present
    * Use both hands
    * Always have a towel nearby in case of messes

***
# **Paragraphs**

Do I contradict myself?

Very well then I contradict myself,

(I am large, I contain multitudes.)

	