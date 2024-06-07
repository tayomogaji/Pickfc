import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class InfoService {

  tokens: string[] = [

    "Awarded after every 2nd consecutive win, hit chips can be used to put pressure on, or eliminate rival players ranked below you. 2 life points are deducted from the player you choose to hit if the team they picked fails to win their fixture.",

    "To place a hit, click on the status of the player you want to target.",

    "Hits can be called off here",

    "Awarded after ever win, boost chips can be used to quickly climb up the rankings. As well as players still with life in the game, eliminated players also count towards your total round score. Your total round score is then multiplied by the amount of boost chips you choose to play."
  ];

  endGame: string[] = [
    "Erase all progress made by players in this game, remove inactive players and generate a new game code.",

    "Permanently remove this game and all instances associated with it."
  ];

  constructor() { }
}
