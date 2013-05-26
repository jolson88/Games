package com.owlxgames.oscar.systems;

import java.util.Random;

import com.artemis.Entity;
import com.artemis.managers.GroupManager;
import com.artemis.systems.VoidEntitySystem;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.Groups;
import com.owlxgames.oscar.components.BubbleComponent;
import com.owlxgames.oscar.components.TransformComponent;

public class GameLogicSystem extends VoidEntitySystem {
	private static final int ROWS = 11;
	private static final int COLUMNS = 9;
	
	private Random _random;
	private GroupManager _groupManager;
	
	@Override
	protected void initialize() {
		_random = new Random();
		_groupManager = world.getManager(GroupManager.class);
		createBubbleEntities();		
	}
	
	@Override
	protected void processSystem() {
		// TODO Auto-generated method stub
	}

	private void createBubbleEntities() {
		Entity e;
		for(int col = 0; col < COLUMNS; col++) {
			for(int row = 0; row < ROWS; row++) {
				e = world.createEntity();
				e.addComponent(new TransformComponent(0,0));
				e.addComponent(new BubbleComponent(col, row, getRandomBubbleKind()));
				e.addToWorld();
				_groupManager.add(e, Groups.bubbles);
			}
		}
	}
	
	private BubbleKind getRandomBubbleKind() {
		int pick = _random.nextInt(BubbleKind.values().length);
		return BubbleKind.values()[pick];
	}
}
