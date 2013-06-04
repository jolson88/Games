package com.owlxgames.oscar;

import java.util.Iterator;
import java.util.NoSuchElementException;

import com.owlxgames.oscar.components.BubbleComponent;

public class BubbleIterator implements Iterator<BubbleComponent> {
	private BubbleComponent _currentRoot;
	private BubbleComponent _current;
	private boolean _started;
	private Mode _mode;
	
	public BubbleIterator(BubbleComponent startBubble) {
		_currentRoot = startBubble;
		_current = _currentRoot;
		_started = false;
		_mode = Mode.Both;
	}
	
	public BubbleIterator(BubbleComponent startBubble, Mode mode) {
		_currentRoot = startBubble;
		_current = _currentRoot;
		_started = false;
		_mode = mode;
		
	}
	@Override
	public boolean hasNext() {
		switch (_mode) {
			case Vertical: return hasVerticalNext();
			case Horizontal: return hasHorizontalNext();
			default: return hasVerticalNext() || hasHorizontalNext();
		}
	}

	@Override
	public BubbleComponent next() {
		if (!_started) {
			_started = true;
			return _current;
		}
		else if (_current.aboveBubble != null && _mode != Mode.Horizontal) {
			_current = _current.aboveBubble;
			return _current;
		}
		else if (_currentRoot.rightBubble != null && _mode != Mode.Vertical) {
			_current = _currentRoot.rightBubble;
			_currentRoot = _current;
			return _current;
		}
		else {
			throw new NoSuchElementException();
		}
	}

	@Override
	public void remove() {
		throw new RuntimeException("Not supported.");
	}
	
	private boolean hasVerticalNext() {
		return (_current.aboveBubble != null) || (!_started && _current != null);
	}
	
	private boolean hasHorizontalNext() {
		return (_current.rightBubble != null) || (!_started && _current != null);	
	}
	
	public enum Mode {
		Vertical,
		Horizontal,
		Both
	}
}
