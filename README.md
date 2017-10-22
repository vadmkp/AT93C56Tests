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
/*
  Blink

  Turns an LED on for one second, then off for one second, repeatedly.

  Most Arduinos have an on-board LED you can control. On the UNO, MEGA and ZERO
  it is attached to digital pin 13, on MKR1000 on pin 6. LED_BUILTIN is set to
  the correct LED pin independent of which board is used.
  If you want to know what pin the on-board LED is connected to on your Arduino
  model, check the Technical Specs of your board at:
  https://www.arduino.cc/en/Main/Products

  modified 8 May 2014
  by Scott Fitzgerald
  modified 2 Sep 2016
  by Arturo Guadalupi
  modified 8 Sep 2016
  by Colby Newman

  This example code is in the public domain.

  http://www.arduino.cc/en/Tutorial/Blink
*/

// the setup function runs once when you press reset or power the board
void setup() {
  // initialize digital pin LED_BUILTIN as an output.
  pinMode(LED_BUILTIN, OUTPUT);
}

// the loop function runs over and over again forever
void loop() {
  digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on (HIGH is the voltage level)
  delay(1000);                       // wait for a second
  digitalWrite(LED_BUILTIN, LOW);    // turn the LED off by making the voltage LOW
  delay(1000);                       // wait for a second
}
```

***

## **python**

```python
s = "Python syntax highlighting"
print s
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



```
Here's our logo (hover to see the title text):

Inline-style: 
![Logo PNG not found ](https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon484.png "Logo Title Text 1")

Reference-style: 
![To powinno byc logo][logo]

[logo]: https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon48.png "Logo Title Text 2"

```

Here's our logo (hover to see the title text):

Inline-style: 
![Logo PNG not found ](https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon484.png "Logo Title Text 1")

Reference-style: 
![To powinno byc logo][logo]

[logo]: https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon48.png "Logo Title Text 2"

[Google](https://google.com" target="_blank)


<a href="http://example.com/" target="_blank">
<img src="https://github.com/adam-p/markdown-here/raw/master/src/common/images/icon48.png" alt="HTML tutorial">
</a>






































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

***

## **Syntax Highlighting**

Inline `code` has `back-ticks around` it.

















***

## **Tables**

Colons can be used to align columns.

| Tables        | Are           | Cool  |
| ------------- |:-------------:| -----:|
| col 3 is      | right-aligned | $1600 |
| col 2 is      | centered      |   $12 |
| zebra stripes | are neat      |    $1 |

There must be at least 3 dashes separating each header cell.
The outer pipes (|) are optional, and you don't need to make the 
raw Markdown line up prettily. You can also use inline Markdown.

Markdown | Less | Pretty
--- | :---: | ---:
*Still* | `renders` | **nicely**
1 | 2 | 3





***

## **Inline HTML**

<dl>
  <dt>Definition list</dt>
  <dd>Is something people use sometimes.</dd>

  <dt>Markdown in HTML</dt>
  <dd>Does *not* work **very** well. Use HTML <em>tags</em>.</dd>
</dl>

***

## **Horizontal Rule**

Three or more...

3 * Ninus

---

3 * Hyphens

***

3 * Asterisks

___

3 * Underscores

***

## **Line Breaks**

Here's a line for us to start with.

This line is separated from the one above by two newlines, so it will be a *separate paragraph*.

This line is also a separate paragraph, but...
This line is only separated by a single newline, so it's a separate line in the *same paragraph*.

***

## **YouTube Videos**

<a href="https://youtu.be/3zoy1deTVu0
" target="_blank"><img src="https://img.youtube.com/vi/3zoy1deTVu0/0.jpg" 
alt="Click here to open video" width="240" height="180" border="10" /></a>

**OR**

[![Click here to open video](https://img.youtube.com/vi/3zoy1deTVu0/0.jpg)](https://youtu.be/3zoy1deTVu0)


<a href="https://www.youtube.com/watch?feature=player_embedded&v=3zoy1deTVu0
" target="_blank"><img src="https://img.youtube.com/vi/3zoy1deTVu0/2.jpg" 
alt="IMAGE ALT TEXT HERE" width="240" height="180" border="10" /></a>


<iframe width="420" height="315"
src="https://www.youtube.com/embed/XGSy3_Czz8k">
</iframe>

<iframe width="560" height="315" src="https://www.youtube.com/embed/ImVK5cGVrpQ" frameborder="0" allowfullscreen></iframe>

***

[![Everything Is AWESOME](http://i.imgur.com/Ot5DWAW.png)](https://youtu.be/StTqXEQ2l-Y?t=35s "Everything Is AWESOME" target="_blank")
