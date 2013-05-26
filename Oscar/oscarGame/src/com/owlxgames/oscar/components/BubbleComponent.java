package com.owlxgames.oscar.components;

import com.artemis.Component;
import com.owlxgames.oscar.BubbleKind;

public class BubbleComponent extends Component {
	public int column;
	public int row;
	public BubbleKind kind;
	public boolean isSelected;
	
	public BubbleComponent(int column, int row, BubbleKind kind) {
		this.column = column;
		this.row = row;
		this.kind = kind;
		this.isSelected = false;
	}
}
