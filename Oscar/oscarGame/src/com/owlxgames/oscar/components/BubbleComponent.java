package com.owlxgames.oscar.components;

import com.artemis.Component;
import com.owlxgames.oscar.BubbleKind;

public class BubbleComponent extends Component {
	public BubbleComponent leftBubble;
	public BubbleComponent rightBubble;
	public BubbleComponent aboveBubble;
	public BubbleComponent belowBubble;
	public BubbleKind kind;
	public boolean isSelected;
	public boolean isRemoved;
	public boolean isRoot;
	public int column;
	
	public BubbleComponent() {
		this.kind = BubbleKind.Green;
	}
	
	public BubbleComponent(BubbleKind kind) {
		this.kind = kind;
	}
}
