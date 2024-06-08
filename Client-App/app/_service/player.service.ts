import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Player } from '../_model/player';

@Injectable({
  providedIn: 'root'
})
export class PlayerService {

  private readonly url: string = environment.baseUrl;
  public readonly maxLife: number = 3; 

  constructor(private http: HttpClient) { }

  public player(id: number): Observable<Player> {
    return this.http.get<Player>(`${this.url}api/Player/Player?id=${id}`);
  }

  public players(gameid: number): Observable<Player[]> {
    return this.http.get<Player[]>(`${this.url}api/Player/Players?gameid=${gameid}`)
  }

  public edit(player: Player): Observable<Player> {
    return this.http.put<Player>(`${this.url}api/Player/Edit`, player);
  }

  public exist(gameId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Player/Exist?gameId=${gameId}`);
  }

  public active(gameId: number): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Player/Active?gameId=${gameId}`);
  }

  public delete(id: number): Observable<Player> {
    return this.http.delete<Player>(`${this.url}api/Player/Delete?id=${id}`);
  }
}
