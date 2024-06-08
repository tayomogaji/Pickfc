import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Team } from '../_model/team';

@Injectable({
  providedIn: 'root'
})
export class TeamService {

  private readonly url: string = environment.baseUrl;
  private readonly picPaths: string[] = ['/assets/team/', 'team/_placeholder.png']

  constructor(private http: HttpClient) { }

  public team(id: number): Observable<Team> {
    return this.http.get<Team>(`${this.url}api/Team/Team?id=${id}`);
  }

  public teams(compId: number): Observable<Team[]> {
    return this.http.get<Team[]>(`${this.url}api/Team/Teams?compId=${compId}`);
  }

  public all(): Observable<Team[]> {
    return this.http.get<Team[]>(`${this.url}api/Team/All`);
  }

  public fixtureless(roundid: number): Observable<Team[]> {
    return this.http.get<Team[]>(`${this.url}api/Team/Fixtureless?roundid=${roundid}`)
  }

  public addedit(team: Team): Observable<Team> {
    return team.id === 0 ? this.http.post<Team>(`${this.url}api/Team/Add`, team) : this.http.put<Team>(`${this.url}api/Team/Edit`, team);
  }

  public delete(id: number): Observable<Team> {
    return this.http.delete<Team>(`${this.url}api/Team/Delete?id=${id}`);
  }

  public compadd(team: Team): Observable<Team> {
    return this.http.post<Team>(`${this.url}api/Team/CompAdd`, team);
  }

  public compdelete(compid: number, teamid: number): Observable<Team> {
    return this.http.delete<Team>(`${this.url}api/Team/CompDelete?compid=${compid}&teamid=${teamid}`);
  }

  public exist(name: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.url}api/Team/Exist?name=${name}`);
  }

  //public pic(p: string): string {
  //  if (p === this.picPaths[0] || p === '')
  //    p = this.picPaths[1];
  //  return  p
  //}
}
