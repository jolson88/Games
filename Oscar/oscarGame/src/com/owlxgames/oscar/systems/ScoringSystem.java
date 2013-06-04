package com.owlxgames.oscar.systems;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.owlxgames.oscar.BubblesPoppedEvent;
import com.owlxgames.oscar.ScoreChangedEvent;
import com.owlxgames.oscar.components.GameStateComponent;
import com.squareup.otto.Bus;
import com.squareup.otto.Subscribe;

public class ScoringSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<GameStateComponent> _gameStateMapper;
	
	private Bus _bus;
	private GameStateComponent _gameState;
	
	@SuppressWarnings("unchecked")
	public ScoringSystem(Bus bus) {
		super(Aspect.getAspectForAll(GameStateComponent.class));
		_bus = bus;
		_bus.register(this);
	}

	@Subscribe public void onBubblesPopped(BubblesPoppedEvent event) {
		int baseValue = event.bubbles.size() * 5;
		int points = baseValue * event.bubbles.size();
	
		_gameState.score += points;
		_bus.post(new ScoreChangedEvent(_gameState.score, points));
	}
	
	@Override
	protected void inserted(Entity e) {
		_gameState = _gameStateMapper.get(e);
	}
	
	@Override
	protected void process(Entity e) { }
}
