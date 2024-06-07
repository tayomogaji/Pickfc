import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Game } from '../_model/game';

@Injectable({
  providedIn: 'root'
})
export class GameService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public game(id: number): Observable<Game> {
    return this.http.get<Game>(`${this.url}api/Game/Game?id=${id}`);
  }

  public public(compId: number): Observable<Game> {
    return this.http.get<Game>(`${this.url}api/Game/Public?compId=${compId}`);
  }

  public users(): Observable<Game[]> {
    return this.http.get<Game[]>(`${this.url}api/Game/Users`);
  }

  public addedit(game: Game): Observable<Game> {
    return game.id === 0 ? this.http.post<Game>(`${this.url}api/Game/Add`, game) : this.http.put<Game>(`${this.url}api/Game/Edit`, game);
  }

  public join(code: string): Observable<Game> {
    return this.http.get<Game>(`${this.url}api/Game/Join?code=${code}`);
  }

  public delete(id: number): Observable<Game> {
    return this.http.delete<Game>(`${this.url}api/Game/Delete?id=${id}`);
  }

  public reset(game: Game): Observable<Game> {
    return this.http.put<Game>(`${this.url}api/Game/reset`, game);
  }

  public validCode(code: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Game/ValidCode?code=${code}`);
  }
}
