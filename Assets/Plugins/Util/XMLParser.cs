/* -------------------------------------------------------------------

 Author:
   Cameron T. Owen
  
 Contact:
 	cameron.t.owen at gmail dot com

 Copyright (c) 2011,	Wayward Logic, www.waywardlogic.com
 						Cameron T. Owen

 All rights reserved.

 Redistribution and use in source and binary forms, with or without 
 modification, are permitted provided that the following conditions 
 are met:

    * Redistributions of source code must retain the above 
      copyright notice, this list of conditions and the 
      following disclaimer.
    * Redistributions in binary form must reproduce the above 
      copyright notice, this list of conditions and the following
      disclaimer in the documentation and/or other materials 
      provided with the distribution.
    * Neither the name of Wayward Logic nor the names of its
      contributors may be used to endorse or promote products 
      derived from this software without specific prior written 
      permission.

 THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
 OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
 SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT 
 LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
 DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
 THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
 (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE 
 OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum XMLTokenType          {None,Declaration,EntityElement,StartElement,EndElement,Attribute,Text,Entity}
public enum XMLParserAttrbuteMode {Name,Assignment,Value}
public enum XMLNodeType           {Text,Element}

/// <summary>
/// XML Attribute Type 
/// </summary>
[System.Serializable]
public struct XMLAttribute
{
	/// <summary>
	/// Name of the attribute
	/// </summary>
	public string name;
	
	/// <summary>
	/// Value of the attribute
	/// </summary>
	public string value;
	
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLAttribute"/> struct.
	/// </summary>
	/// <param name='name'>
	/// Name of the attribute.
	/// </param>
	/// <param name='value'>
	/// Value of the attribute.
	/// </param>
	public XMLAttribute (string name, string value)
	{
		this.name = name;
		this.value = value;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLAttribute"/> struct.
	/// </summary>
	/// <param name='name'>
	/// Name of the attribute.
	/// </param>
	public XMLAttribute (string name)
	{
		this.name = name;
		this.value = "";
	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="XMLAttribute"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents the current <see cref="XMLAttribute"/>.
	/// </returns>
	public override string ToString ()
	{
		return value;
	}
	
}


/// <summary>
/// Interface that all XML datatypes impliment. 
/// </summary>
public interface IXMLNode
{
	
	/// <summary>
	/// Gets or sets the value of the node.
	/// </summary>
	/// <value>
	/// The value.
	/// </value>
	string value { get; set; }
	
	/// <summary>
	/// Gets the type.
	/// </summary>
	/// <value>
	/// The type.
	/// </value>
	XMLNodeType type { get; }
	
	/// <summary>
	/// Gets the parent.
	/// </summary>
	/// <value>
	/// The parent IXMLNode in the hierarchy.
	/// </value>
	IXMLNode Parent { get; }
	
	/// <summary>
	/// Gets or sets the children.
	/// </summary>
	/// <value>
	/// The children IXMLNodes of this node.
	/// </value>
	List<IXMLNode> Children { get; set; }

	List<XMLAttribute> Attributes { get; set; }
	
}

/// <summary>
/// XMLText node is used to represent text content in an XML document.
/// </summary>
[System.Serializable]
public class XMLText : IXMLNode
{
	
	protected string valueString;
	protected IXMLNode parentNode;
	
	/// <summary>
	/// Gets or sets the value.
	/// </summary>
	/// <value>
	/// The value string contains the text content of this node.
	/// </value>
	public string value {
		get { return valueString; }
		set { valueString = value; }
	}
	
	/// <summary>
	/// Gets the type.
	/// </summary>
	/// <value>
	/// Will always return XMLNodeType.Text
	/// </value>
	public XMLNodeType type {
		get { return XMLNodeType.Text; }
	}
	
	/// <summary>
	/// Gets the parent IXMLNode.
	/// </summary>
	/// <value>
	/// The parent node object.
	/// </value>
	public IXMLNode Parent {
		get { return parentNode; }
	}
	
	/// <summary>
	/// Gets or sets the child list.
	/// </summary>
	/// <value>
	/// The list of child nodes. Will always be an empty list. Settig the list will have no effect.
	/// </value>
	public List<IXMLNode> Children {
		get { return new List<IXMLNode> (); }
		set { }
	}
	
	/// <summary>
	/// Gets or sets the attribute list.
	/// </summary>
	/// <value>
	/// The list of this node's attributes. Will always be an empty list.
	/// </value>
	public List<XMLAttribute> Attributes {
		get { return new List<XMLAttribute> (); }
		set { }
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLText"/> class.
	/// </summary>
	/// <param name='text'>
	/// The text of the node.
	/// </param>
	/// <param name='parent'>
	/// This node's parent node. Should not be null for text nodes.
	/// </param>
	public XMLText (string text, IXMLNode parent)
	{
		valueString = text;
		parentNode = parent;
		if (parent != null) {
			parent.Children.Add (this);
		}
	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="XMLText"/>.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String"/> that represents the current <see cref="XMLText"/>.
	/// </returns>
	public override string ToString ()
	{
		return valueString;
	}
}

/// <summary>
/// XMLElement class is a container class for other XMLElements, XMLText nodes and XMLAttributes. 
/// </summary>
[System.Serializable]
public class XMLElement : IXMLNode
{

	protected string valueString;
	protected IXMLNode parentNode;
	protected List<IXMLNode> childList;
	protected List<XMLAttribute> attributeList;
	
	/// <summary>
	/// Gets or sets the name value.
	/// </summary>
	/// <value>
	/// The name value of this XML Tag.
	/// </value>
	public string value {
		get { return valueString; }
		set { valueString = value; }
	}
	
	/// <summary>
	/// Gets the type.
	/// </summary>
	/// <value>
	/// Will always return XMLNodeType.Element
	/// </value>
	public XMLNodeType type {
		get { return XMLNodeType.Element; }
	}
	
	/// <summary>
	/// Gets the parent node.
	/// </summary>
	/// <value>
	/// The parent XMLElement of this node or null if this is the root node.
	/// </value>
	public IXMLNode Parent {
		get { return parentNode; }
	}

	/// <summary>
	/// Gets or sets the child list.
	/// </summary>
	/// <value>
	/// The list of child nodes. Will always be an empty list.
	/// </value>
	public List<IXMLNode> Children {
		get { return childList; }
		set { childList = value; }
	}
	
	/// <summary>
	/// Gets or sets the attribute list.
	/// </summary>
	/// <value>
	/// The list of this node's attributes.
	/// </value>
	public List<XMLAttribute> Attributes {
		get { return attributeList; }
		set { attributeList = value; }
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLElement"/> class.
	/// </summary>
	/// <param name='name'>
	/// Tage Name of this XMLElement.
	/// </param>
	/// <param name='parent'>
	/// The Parent node.
	/// </param>
	/// <param name='children'>
	/// List of child nodes
	/// </param>
	/// <param name='attributes'>
	/// List of attributes.
	/// </param>
	public XMLElement (string name, IXMLNode parent, List<IXMLNode> children, List<XMLAttribute> attributes)
	{
		valueString = name;
		parentNode = parent;
		childList = children;
		attributeList = attributes;
		if (parent != null) {
			parent.Children.Add (this);
		}
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLElement"/> class.
	/// </summary>
	/// <param name='name'>
	/// Tage Name of this XMLElement.
	/// </param>
	/// <param name='parent'>
	/// The Parent node.
	/// </param>
	/// <param name='children'>
	/// List of child nodes
	/// </param>
	public XMLElement (string name, IXMLNode parent, List<IXMLNode> children)
	{
		valueString = name;
		parentNode = parent;
		childList = children;
		attributeList = new List<XMLAttribute> ();
		if (parent != null) {
			parent.Children.Add (this);
		}
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLElement"/> class.
	/// </summary>
	/// <param name='name'>
	/// Tage Name of this XMLElement.
	/// </param>
	/// <param name='parent'>
	/// The Parent node.
	/// </param>
	public XMLElement (string name, IXMLNode parent)
	{
		valueString = name;
		parentNode = parent;
		childList = new List<IXMLNode> ();
		attributeList = new List<XMLAttribute> ();
		if (parent != null) {
			parent.Children.Add (this);
		}
	}
	
}


/// <summary>
/// XMLParser is a static class used to parse XML formmated strings into a hierarchy of IXMLNode objects.
/// </summary>
public class XMLParser
{

	protected XMLElement rootElement;
	protected XMLElement currentElement;
	protected bool rootflag;
	protected List<XMLAttribute> attributeList;
	protected string xmlString;
	
	/// <summary>
	/// Gets the XML Root element.
	/// </summary>
	/// <value>
	/// The root XMLElement of the last parsed xml string.
	/// </value>
	public XMLElement XMLRootElement {
		get {
			return rootElement;
		}
	}
	
	/// <summary>
	/// Gets the last XML string that was parsed by the parser or sets a new one for future parse calls.
	/// </summary>
	/// <value>
	/// The XML string.
	/// </value>
	public string XMLString {
		get {
			return xmlString;
		} set {
			xmlString = value;
			rootflag = false;
		}
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="XMLParser"/> class.
	/// </summary>
	/// <param name='xmlString'>
	/// XML formatted string to parse.
	/// </param>
	public XMLParser(string xmlString) 
	{
		this.xmlString = xmlString;	
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="XMLParser"/> class.
	/// </summary>
	public XMLParser() 
	{
		this.xmlString = "";
	}
	
	/// <summary>
	/// Parse the specified xmlString.
	/// </summary>
	/// <param name='xmlString'>
	/// Xml string.
	/// </param>
	/// <returns>
	/// A <see cref="IXMLNode"/> hierarchy of the given XML formatted string.
	/// </returns>
	public virtual XMLElement Parse(string xmlString) {
		this.xmlString = xmlString;
		return Parse();
	}
	
	/// <summary>
	/// Parse the previously set xmlString.
	/// </summary>
	/// <returns>
	/// A <see cref="IXMLNode"/> hierarchy of the given XML formatted string.
	/// </returns>
	public XMLElement Parse () {
		
		rootflag = false;
		
		//this.xmlString = xmlString;
		
		string contentString = "";
		string elementName = "";
		string attributeName = "";
		string entitySuspendedContent = "";
		
		XMLTokenType currentToken = XMLTokenType.None;
		XMLTokenType previousToken = XMLTokenType.None;
		
		XMLParserAttrbuteMode attributeMode = XMLParserAttrbuteMode.Name;
		
		attributeList = new List<XMLAttribute> ();
		
		int i = 0;
		int xmlStringLength = xmlString.Length;
		
		// TOKENIZE XML FORMATTED STRING
		while (i < xmlStringLength) {
			
			char c = xmlString[i];
			
			switch (c) {
			
			// TAG TOKEN START
			case '<':
				
				previousToken = currentToken;
				// switching token types...
				// deal with passive token handlers on active token start
				switch (previousToken) {
				case XMLTokenType.Entity:
					EntityHandler (contentString);
				break;
				case XMLTokenType.Text:
					TextHandler (contentString);
				break;
				}

				
				// What kind of tag start?
				switch (xmlString[i + 1]) {
				case '?':
					currentToken = XMLTokenType.Declaration;
					i++;
				break;
				case '!':
					currentToken = XMLTokenType.EntityElement;
					i++;
				break;
				case '/':
					currentToken = XMLTokenType.EndElement;
					i++;
				break;
				default: // Non specific token start, decide type based on previous token
					switch (previousToken) {
					case XMLTokenType.Declaration:
						currentToken = XMLTokenType.Declaration;
					break;
					case XMLTokenType.EntityElement:
						currentToken = XMLTokenType.EntityElement;
					break;
					default:
						currentToken = XMLTokenType.StartElement;
					break;
					}
				break;
					
				}

				// reset content string for new token
				contentString = "";
				
			break;
			
			// TAG TOKEN END
			case '>':
				
				// switching token types...
				previousToken = currentToken;
				
				switch (currentToken) {
				
				case XMLTokenType.Declaration:
					DeclarationHandler (contentString);
				break;
				case XMLTokenType.EntityElement:
					EntityHandler (contentString);
				break;
				case XMLTokenType.StartElement:
					StartElementHandler (contentString);
					// handle special case for solo tags
					if (xmlString[i - 1] == '/') {
						EndElementHandler(contentString);
					}
				break;
				case XMLTokenType.EndElement:
					EndElementHandler (contentString);
				break;
				case XMLTokenType.Attribute:
					StartElementHandler (elementName);
					previousToken = XMLTokenType.StartElement;
				break;
				}
				
				// Default to text token untill we find something different...
				contentString = "";
				currentToken = XMLTokenType.Text;
				
			break;
			
			// WHITESPACE & ATTRIBUTE START
			case ' ':
				
				// This ought to remove needless whitespace but it mucks up
				// entity parsing, not sure why :( - probably don't need
				// to agressively stip whitsepace for game use though.
				/*
				int contentStringLength = contentString.Length;
				if (contentStringLength > 1) {
					if (contentString[contentStringLength - 2] == ' ')
						break; // break and ignore extra whitespace
					
				}
				*/
				
				switch (currentToken) {
				case XMLTokenType.StartElement:
					previousToken = currentToken;
					currentToken = XMLTokenType.Attribute;
					elementName = contentString;
					contentString = "";
					attributeMode = XMLParserAttrbuteMode.Name;
				break;
				case XMLTokenType.Text:
					contentString += c;
				break;
				case XMLTokenType.Attribute:
					if (attributeMode == XMLParserAttrbuteMode.Value) {
						contentString += c;
					}
				break;
				}

				break;
			
			// ATTRIBUTE ASSIGNMENT
			case '=':
				switch (currentToken) {
				case XMLTokenType.Attribute:
					switch (attributeMode) {
					case XMLParserAttrbuteMode.Name:
						attributeName = contentString.Trim();
						contentString = "";
						attributeMode = XMLParserAttrbuteMode.Assignment;
					break;
					case XMLParserAttrbuteMode.Value:
						contentString += c;
					break;
					}

				break;
				default:
					contentString += c;
					break;
				}

				break;
			
			// ATTRIBUTE VALUE
			case '"':
				switch (currentToken) {
				case XMLTokenType.Attribute:
					switch (attributeMode) {
					// Start Value
					case XMLParserAttrbuteMode.Assignment:
						attributeMode = XMLParserAttrbuteMode.Value;
					break;
					// End Value
					case XMLParserAttrbuteMode.Value:
						AttributeHandler (attributeName, contentString);
						contentString = "";
						attributeMode = XMLParserAttrbuteMode.Name;
					break;
					}

				break;
				}

				break;
			
			// ENTITY REFERENCE START
			case '&':
				previousToken = currentToken; // siwtch to entity mode
				currentToken = XMLTokenType.Entity;
				entitySuspendedContent = contentString; // save current content while in entity mode
				contentString = "";  // clear content for recording entity
			break;
				
			// ENTITY REFERENCE END
			case ';':
				if(currentToken == XMLTokenType.Entity) {
					currentToken = previousToken; // swicth back to last token mode.
					contentString = entitySuspendedContent + ParseEntityReference(contentString); // restore content string with parsed entity
				} else {
					contentString += c;
				}
			break;
			
			// DEFAULT CONTENT
			default:	
				contentString += c; // jsut add it to the content string...
			break;
				
			}
			
			i++;
		}
				
		return rootElement;
		
	}
	
	/// <summary>
	/// Handles XML decleration. Base implimentation does nothing.
	/// </summary>
	/// <param name='content'>
	/// Content of the entity tag
	/// </param>
	protected virtual void DeclarationHandler (string content)
	{
		// does nothing
	}
	
	/// <summary>
	/// Handles entitys like comments. Base implimentation does nothing.
	/// </summary>
	/// <param name='content'>
	/// Content of the entity tag
	/// </param>
	protected virtual void EntityHandler (string content)
	{
		// does nothing
	}

	/// <summary>
	/// Handles opening tags.
	/// </summary>
	/// <param name='tagName'>
	/// Tag name.
	/// </param>
	protected virtual void StartElementHandler (string tagName)
	{
		
		// placeholder for new element.
		XMLElement newElement; // no assignment is bad form but necessary

		if (!rootflag) { // need to setup root element...
			newElement = new XMLElement (tagName.Trim(), null);
			rootElement = newElement;
			rootflag = true;
		} else { // standard new element creations...
			newElement = new XMLElement (tagName.Trim(), currentElement);
		}
		
		// Add attributes we've encounted thus far
		int i = 0;
		int attributeCount = attributeList.Count;
		while (i < attributeCount) {
			newElement.Attributes.Add (attributeList[i]);
			i++;
		}
		
		// clear saved attribute list
		attributeList = new List<XMLAttribute> ();
		
		// advance current element
		currentElement = newElement;
		
	}

	/// <summary>
	/// Handles end tags.
	/// </summary>
	/// <param name='tagName'>
	/// Tag name.
	/// </param>
	protected virtual void EndElementHandler (string tagName)
	{
		if (rootflag) {
			if (rootElement != currentElement) {
				currentElement = (XMLElement)currentElement.Parent;
			}
		}
	}
	
	/// <summary>
	/// Handles attribute name = value pairs.
	/// </summary>
	/// <param name='name'>
	/// Name of the attribute.
	/// </param>
	/// <param name='value'>
	/// Value of the attribute.
	/// </param>
	protected virtual void AttributeHandler (string name, string value)
	{
		attributeList.Add (new XMLAttribute (name.Trim(), value));
	}
	
	/// <summary>
	/// Handles text content inetween tags. Combines neighbouring text nodes where possible.
	/// </summary>
	/// <param name='text'>
	/// The text content of the node.
	/// </param>
	protected virtual void TextHandler (string text)
	{
		// only if usable text and root flag set
		if (rootflag && text.Trim ().Length > 0) {
			
			// check for text nodes made jsut before this append instead of creating a new node
			if (currentElement.Children.Count != 0) {
				IXMLNode lastOfCurrent = currentElement.Children[currentElement.Children.Count - 1];
				if (lastOfCurrent.type == XMLNodeType.Text) {
					lastOfCurrent.value += text;
					return; // exit early
				}
			}
			
			// If we get here we need a new text node...
			new XMLText (text, currentElement);
			
		}
	}
	
	
	public static char ParseEntityReference(string entity)
	{
		
		switch(entity) {
		case "lt"  : return '<';	
		case "gt"  : return '>';	
		case "quot": return '"';
		case "apos": return '\'';	
		case "amp" : return '&';
		}
		
		return '\0'; // return unicode 0 for unknown entities
		
	}
	
	public static string GetEntityReference(char c)
	{
		switch(c) {
		case '<' : return "&lt;";
		case '>' : return "&gt;";
		case '"' : return "&quot;";
		case '\'': return "&apos;";
		case '&' : return "&amp;";
		}
		return c.ToString();
	}
	
}