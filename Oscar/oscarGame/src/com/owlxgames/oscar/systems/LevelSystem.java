package com.owlxgames.oscar.systems;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.managers.TagManager;
import com.artemis.systems.EntityProcessingSystem;
import com.owlxgames.oscar.BubbleIterator;
import com.owlxgames.oscar.BubblesPoppedEvent;
import com.owlxgames.oscar.NewGameEvent;
import com.owlxgames.oscar.NewLevelEvent;
import com.owlxgames.oscar.Tags;
import com.owlxgames.oscar.components.BubbleComponent;
import com.owlxgames.oscar.components.GameStateComponent;
import com.squareup.otto.Bus;
import com.squareup.otto.Subscribe;

public class LevelSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<BubbleComponent> _bubbleMapper;
	
	private BubbleComponent _rootBubble;
	private Bus _bus;
	private GameStateComponent _gameState;
	
	@SuppressWarnings("unchecked")
	public LevelSystem(Bus bus) {
		super(Aspect.getAspectForAll(BubbleComponent.class));
		_bus = bus;
		_bus.register(this);
	}
	
	@Override
	protected void initialize() {
		ComponentMapper<GameStateComponent> _gameStateMapper = world.getMapper(GameStateComponent.class);
		TagManager _tagManager = world.getManager(TagManager.class);
		
		Entity e = _tagManager.getEntity(Tags.gameState);
		_gameState = _gameStateMapper.get(e);
	}
	
	@Override
	protected void inserted(Entity e) {
		BubbleComponent bubble = _bubbleMapper.get(e);
		if (bubble.isRoot && bubble.column == 0) {
			_rootBubble = bubble;
		}
	}
	
	@Subscribe
	public void onNewGame(NewGameEvent evt) {
		_gameState.level = 1;
		_gameState.targetScore = calculateLevelTargetScore(1);
	}
	
	@Subscribe
	public void onBubblesPopped(BubblesPoppedEvent evt) {
		if (!hasMovesRemaining()) {
			_gameState.score += bonusPointsAchieved();
			if (_gameState.score >= _gameState.targetScore){ 
				_bus.post(new NewLevelEvent());
			}
		}
	}
	
	@Subscribe
	public void onNewLevel(NewLevelEvent evt) {
		_gameState.level++;
		_gameState.targetScore = calculateLevelTargetScore(_gameState.level);
	}
	
	private int bonusPointsAchieved() {
		// Only get bonus points if you have less than 10 bubbles remaining
		int remainingBubbles = bubblesRemaining();
		return Math.max(0, 2000 - (remainingBubbles * (2000 / 10)));
	}
	
	private int calculateLevelTargetScore(int level) {
		return 1000 + ((level - 1) * 1800) + (500 * (level / 5));
	}
	
	private int bubblesRemaining() {
		int remaining = 0;
		
		BubbleComponent bubble;
		BubbleIterator iter = new BubbleIterator(_rootBubble);
		while (iter.hasNext()) {
			bubble = iter.next();
			if (!bubble.isRemoved) {
				remaining++;
			}
		}
		return remaining;
	}
	
	private boolean hasMovesRemaining() {
		BubbleComponent bubble;
		BubbleIterator iter = new BubbleIterator(_rootBubble);
		while (iter.hasNext()) {
			bubble = iter.next();
			if (matchesAboveBubble(bubble) || matchesRightBubble(bubble)) {
				return true;
			}
		}
		return false;
	}
	
	private boolean matchesAboveBubble(BubbleComponent bubble) {
		return bubble.aboveBubble != null && !bubble.isRemoved && !bubble.aboveBubble.isRemoved && bubble.kind == bubble.aboveBubble.kind;
	}
	
	private boolean matchesRightBubble(BubbleComponent bubble) {
		return bubble.rightBubble != null && !bubble.isRemoved && !bubble.rightBubble.isRemoved && bubble.kind == bubble.rightBubble.kind;
	}
	
	@Override protected void process(Entity e) { }
}
