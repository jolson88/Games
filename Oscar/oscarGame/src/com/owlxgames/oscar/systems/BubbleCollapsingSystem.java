package com.owlxgames.oscar.systems;

import com.artemis.Aspect;
import com.artemis.ComponentMapper;
import com.artemis.Entity;
import com.artemis.annotations.Mapper;
import com.artemis.systems.EntityProcessingSystem;
import com.owlxgames.oscar.BubbleKind;
import com.owlxgames.oscar.BubblesPoppedEvent;
import com.owlxgames.oscar.GameConstants;
import com.owlxgames.oscar.components.BubbleComponent;
import com.squareup.otto.Bus;
import com.squareup.otto.Subscribe;

public class BubbleCollapsingSystem extends EntityProcessingSystem {
	@Mapper ComponentMapper<BubbleComponent> _bubbleMapper;

	private Bus _bus;
	private BubbleComponent[] _rootBubbles;

	@SuppressWarnings("unchecked")
	public BubbleCollapsingSystem(Bus bus) {
		super(Aspect.getAspectForAll(BubbleComponent.class));
		_bus = bus;
		_bus.register(this);
		
		_rootBubbles = new BubbleComponent[GameConstants.columnCount];
	}

	@Subscribe 
	public void onBubblesPopped(BubblesPoppedEvent evt) {
		collapseBubbles();
	}

	@Override
	protected void inserted(Entity e) {
		BubbleComponent bubble = _bubbleMapper.get(e);
		if (bubble.isRoot) {
			_rootBubbles[bubble.column] = bubble;
		}
	}

	@Override
	protected void process(Entity e) {
	}

	private void collapseBubbles() {
		collapseRows();
		collapseColumns();
	}

	private void collapseRows() {
		boolean filled = false;
		
		BubbleComponent bubble;
		for (int column = 0; column < _rootBubbles.length; column++) {
			bubble = _rootBubbles[column];
			while(bubble != null) {
				if (bubble.isRemoved) {
					filled = fillWithReplacementBubble(bubble);
					if (!filled) {
						break;
					}
				}
				bubble = bubble.aboveBubble;
			}
		}
	}
	
	private void collapseColumns() {
		BubbleComponent bubble;
		for (int column = 0; column < _rootBubbles.length; column++) {
			bubble = _rootBubbles[column];
			if (bubble.isRemoved) {
				for (int columnToSwap = column + 1; columnToSwap < _rootBubbles.length; columnToSwap++) {
					if (!_rootBubbles[columnToSwap].isRemoved) {
						swapColumns(bubble, _rootBubbles[columnToSwap]);
						break;
					}
				}
			}
		}
	}
	
	private boolean fillWithReplacementBubble(BubbleComponent bubble) {
		boolean filled = false;
		
		BubbleComponent nextBubble = bubble.aboveBubble;
		while (nextBubble != null) {
			if (!nextBubble.isRemoved) {
				swapBubbles(bubble, nextBubble);
				filled = true;
				break;
			} else {
				nextBubble = nextBubble.aboveBubble;
			}
		}
		
		return filled;
	}

	private void swapColumns(BubbleComponent destinationBubble, BubbleComponent sourceBubble) {
		while (destinationBubble != null) {
			swapBubbles(destinationBubble, sourceBubble);
			destinationBubble = destinationBubble.aboveBubble;
			sourceBubble = sourceBubble.aboveBubble;
		}
	}
	
	private void swapBubbles(BubbleComponent destinationBubble, BubbleComponent sourceBubble) {
		boolean destinationIsRemoved = destinationBubble.isRemoved;
		BubbleKind destinationKind = destinationBubble.kind;
		
		destinationBubble.isRemoved = sourceBubble.isRemoved;
		destinationBubble.kind = sourceBubble.kind;
		sourceBubble.isRemoved = destinationIsRemoved;
		sourceBubble.kind = destinationKind;	
	}
}
