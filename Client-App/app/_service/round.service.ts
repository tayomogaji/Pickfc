import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Round } from '../_model/round';

@Injectable({
  providedIn: 'root'
})
export class RoundService {

  private readonly url: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  public round(id: number): Observable<Round> {
    return this.http.get<Round>(`${this.url}api/Round/Round?id=${id}`);
  }

  public active(compid: number): Observable<Round> {
    return this.http.get<Round>(`${this.url}api/Round/Active?compid=${compid}`);
  }

  public rounds(compid: number): Observable<Round[]> {
    return this.http.get<Round[]>(`${this.url}api/Round/Rounds?compid=${compid}`);
  }

  public addedit(round: Round): Observable<Round> {
    return round.id === 0 ? this.http.post<Round>(`${this.url}api/Round/Add`, round) : this.http.put<Round>(`${this.url}api/Round/Edit`, round);
  }

  public setCurrentRound(round: Round): Observable<Round> {
    return this.http.put<Round>(`${this.url}api/Round/SetCurrentRound`, round);
  }

  public delete(id: number): Observable<Round> {
    return this.http.delete<Round>(`${this.url}api/Round/Delete?id=${id}`);
  }
}
