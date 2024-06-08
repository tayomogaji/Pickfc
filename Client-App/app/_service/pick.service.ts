import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Pick } from '../_model/pick';
import { Player } from '../_model/player';

@Injectable({
  providedIn: 'root'
})
export class PickService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public pick(id: number): Observable<Pick> {
    return this.http.get<Pick>(`${this.url}api/Pick/Pick?id=${id}`);
  }

  public playerPick(player: Player, roundid: number): Observable<Pick> {
    return this.http.get<Pick>(`${this.url}api/Pick/Pick?playerid=${player.id}&gameid=${player.gameID}&roundid=${roundid}`);
  }

  public playerPicks(player: Player): Observable<Pick[]> {
    return this.http.get<Pick[]>(`${this.url}api/Pick/PlayerPicks?playerid=${player.id}&gameid=${player.gameID}`);
  }

  public picks(roundid: number): Observable<Pick[]> {
    return this.http.get<Pick[]>(`${this.url}api/Pick/Picks?roundid=${roundid}`);
  }

  public pickValid(playerid: number, roundid: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Pick/PickValid?playerid=${playerid}&roundid=${roundid}`);
  }

  public pickExist(pick: Pick): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Pick/PickExist?playerid=${pick.playerID}&gameid=${pick.gameID}&roundid=${pick.roundID}`);
  }

  public addedit(pick: Pick): Observable<Pick> {
    return pick.id === 0 ? this.http.post<Pick>(`${this.url}api/Pick/Add`, pick) : this.http.put<Pick>(`${this.url}api/Pick/Edit`, pick);
  }

  public delete(id: number): Observable<Pick> {
    return this.http.delete<Pick>(`${this.url}api/Pick/Delete?id=${id}`)
  }

}
