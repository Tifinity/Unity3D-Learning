using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction {
	void moveForward();
	void moveBack();
	void shoot();
	void turn (float degree);
	void noTurn();
	void noMove();
	int getResult();
}