package com.owlxgames.oscar.components;

import com.artemis.Component;

public class GameStateComponent extends Component {
	public int score;
	public int level;
	public int targetScore;
	
	public GameStateComponent() {
		this.score = 0;
	}
}
