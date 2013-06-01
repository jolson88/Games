package com.owlxgames.oscar;

import java.util.ArrayList;

import com.owlxgames.oscar.components.BubbleComponent;

public class BubblesPoppedEvent {
	public ArrayList<BubbleComponent> bubbles;
	
	public BubblesPoppedEvent(ArrayList<BubbleComponent> bubbles) {
		this.bubbles = bubbles;
	}
}
