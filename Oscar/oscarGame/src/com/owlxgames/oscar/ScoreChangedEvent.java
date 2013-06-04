package com.owlxgames.oscar;

public class ScoreChangedEvent {
	public int newScore;
	public int scoreDelta;
	
	public ScoreChangedEvent(int newScore, int scoreDelta) {
		this.newScore = newScore;
		this.scoreDelta = scoreDelta;
	}
}
