package com.owlxgames.oscar.systems;

import java.util.Random;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.NewGameEvent;
import com.owlxgames.oscar.NewLevelEvent;
import com.owlxgames.oscar.components.BubbleComponent;
import com.squareup.otto.Bus;
import com.squareup.otto.Subscribe;

public class BubblePopulationSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<BubbleComponent> _bubbleMapper;
	
	private BubbleComponent _rootBubble;
	private Random _random;
	
	@SuppressWarnings("unchecked")
	public BubblePopulationSystem(Bus bus) {
		super(Aspect.getAspectForAll(BubbleComponent.class));		
		bus.register(this);
		
		_random = new Random();
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
		repopulateBubbles();
	}
	
	@Subscribe
	public void onNewLevel(NewLevelEvent evt) {
		repopulateBubbles();
	}
	
	private void repopulateBubbles() {
		BubbleComponent rowBubble;
		BubbleComponent columnBubble = _rootBubble;
		while (columnBubble != null) {
			rowBubble = columnBubble;
			while (rowBubble != null) {
				rowBubble.isRemoved = false;
				rowBubble.isSelected = false;
				rowBubble.kind = getRandomBubbleKind();
				
				rowBubble = rowBubble.aboveBubble;
			}
			columnBubble = columnBubble.rightBubble;
		}
	}
	
	private BubbleKind getRandomBubbleKind() {
		int pick = _random.nextInt(BubbleKind.values().length);
		return BubbleKind.values()[pick];
	}
	
	@Override protected void process(Entity e) { }
}
