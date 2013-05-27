package com.owlxgames.oscar.components;

import com.artemis.Component;
import com.owlxgames.oscar.Bubble;

public class BoardComponent extends Component {
	public int columnCount;
	public int rowCount;
	public Bubble[][] bubbles;
	
	public BoardComponent(int columns, int rows) {
		this.columnCount = columns;
		this.rowCount = rows;
		this.bubbles = new Bubble[columns][rows];
	}
}
