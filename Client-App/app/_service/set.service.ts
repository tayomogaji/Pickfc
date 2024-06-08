import { Injectable } from "@angular/core";
import { Game } from "../_model/game";
import { Player } from "../_model/player";
import { User } from "../_model/user";
import { Auth } from "../_model/auth";
import { Comp } from "../_model/comp";
import { Team } from "../_model/team";
import { Pick } from "../_model/pick";
import { Fixture } from "../_model/fixture";
import { Round } from "../_model/round";
import { Notify } from "../_model/notify";
import { Art } from "../_model/art";

@Injectable({
  providedIn: 'root'
})

export class SetService {

  //public ph: string[] = ['comp/_placeholder.png', 'team/_placeholder.png', "art/_placeholder.png"];
  constructor() { }

  public user(): User {
    return {
      id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
      art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
    }
  };

  public art(): Art {
    return { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date()}
  }

  public auth(): Auth {
    return { email: '', password: '', code: '', rememberMe: false, activationCode: false }
  }

  public notify(): Notify {
    return {id: 0, comp: false}
  }

  public player(): Player {
    return {
      id: 0, gameID: 0, pickID: 0, streak: 0, life: 0, pos: 0, pts: 0, roundPts: 0, champs: 0, pickTime: 0, roundPickTime: 0, hitByID: 0, hitsPlayed: 0, hitsTotal: 0, boostPlayed: 0, boostTotal: 0, name: '', hitByPic: '', hitByName: '', eliminated: false, admin: false, active: true, timestamped: new Date(),

      user:{
        id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
        art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
      },

      pick: {
        id: 0, roundID: 0, gameID: 0, playerID: 0, teamID: 0, roundNumber: 0, playerName: '', playerPic: '', result: '', time: new Date(), timestamped: new Date,

        team: {
          id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

          comps: [{
            id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

            admin: {
              id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
              art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
            },

          }]
        }
      }
    }
  };

  public comp(): Comp {
    return {
      id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

      admin: {
        id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
        art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
      }

    }
  }

  public team(): Team {
    return {
      id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

      comps: [{
        id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

        admin: {
          id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
          art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
        },

      }]
    }
  }

  public pick(): Pick
  {
    return {
      id: 0, roundID: 0, gameID: 0, playerID: 0, teamID: 0, roundNumber: 0, playerName: '', playerPic: '', result: '', time: new Date(), timestamped: new Date,

      team: {
        id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

        comps: [{
          id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

          admin: {
            id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
            art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
          },

        }]
      }
    }
  }

  public fixture(): Fixture
  {
    return {
      id: 0, roundID: 0, homeID: 0, awayID: 0, homeResult: '', awayResult: '', resultReset: false, timeStamped: new Date(),

      home: {
        id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

        comps: [{
          id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

          admin: {
            id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
            art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
          },

        }]
      },

      away: {
        id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

        comps: [{
          id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

          admin: {
            id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
            art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
          },

        }]
      }
    }
  }

  public round(): Round {
    return {
      id: 0, compID: 0, number: 1, deadlineMsecs:0, deadlineDays:0, name: '', show: false, current: false, startNotified: false, deadlineNotified: false, start: new Date(), deadline: new Date(), timestamped: new Date(),

      fixtures: [{
        id: 0, roundID: 0, homeID: 0, awayID: 0, homeResult: '', awayResult: '', resultReset: false, timeStamped: new Date(),

        home: {
          id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

          comps: [{
            id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

            admin: {
              id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
              art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
            },

          }]
        },

        away: {
          id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

          comps: [{
            id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

            admin: {
              id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
              art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
            },

          }]
        }
      }],

      picks: [{
        id: 0, roundID: 0, gameID: 0, playerID: 0, teamID: 0, roundNumber: 0, playerName: '', playerPic: '', result: '', time: new Date(), timestamped: new Date(),

        team: {
          id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

          comps: [{
            id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

            admin: {
              id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
              art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
            },

          }]
        }
      }]
    }
  }

  public game(): Game {
    return {
      id: 0, creatorID: 0, compID: 0, name: '', pic: '', code: '', public: false, legacy: false, roundDeadlined: false, deadline: false, deadlineDate: new Date(), timestamped: new Date(),

      comp: {
        id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

        admin: {
          id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
          art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
        }
      },

      creator: {
        id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
        art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
      },

      currentPlayer: {
        id: 0, gameID: 0, pickID: 0, streak: 0, life: 0, pos: 0, pts: 0, roundPts: 0, champs: 0, pickTime: 0, roundPickTime: 0, hitByID: 0, hitsPlayed: 0, hitsTotal: 0, boostPlayed: 0, boostTotal: 0, name: '', hitByPic: '', hitByName: '', eliminated: false, admin: false, active: true, timestamped: new Date(),

        user: {
          id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
          art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
        },

        pick: {
          id: 0, roundID: 0, gameID: 0, playerID: 0, teamID: 0, roundNumber: 0, playerName: '', playerPic: '', result: '', time: new Date(), timestamped: new Date,

          team: {
            id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

            comps: [{
              id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

              admin: {
                id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
                art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
              },

            }]
          }
        }
      },

      admins: [{
        id: 0, gameID: 0, pickID: 0, streak: 0, life: 0, pos: 0, pts: 0, roundPts: 0, champs: 0, pickTime: 0, roundPickTime: 0, hitByID: 0, hitsPlayed: 0, hitsTotal: 0, boostPlayed: 0, boostTotal: 0, name: '', hitByPic: '', hitByName: '', eliminated: false, admin: false, active: true, timestamped: new Date(),

        user: {
          id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
          art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
        },

        pick: {
          id: 0, roundID: 0, gameID: 0, playerID: 0, teamID: 0, roundNumber: 0, playerName: '', playerPic: '', result: '', time: new Date(), timestamped: new Date,

          team: {
            id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

            comps: [{
              id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

              admin: {
                id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
                art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
              },

            }]
          }
        }
      }],

      players: [{
        id: 0, gameID: 0, pickID: 0, streak: 0, life: 0, pos: 0, pts: 0, roundPts: 0, champs: 0, pickTime: 0, roundPickTime: 0, hitByID: 0, hitsPlayed: 0, hitsTotal: 0, boostPlayed: 0, boostTotal: 0, name: '', hitByPic: '', hitByName: '', eliminated: false, admin: false, active: true, timestamped: new Date(),

        user: {
          id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
          art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
        },

        pick: {
          id: 0, roundID: 0, gameID: 0, playerID: 0, teamID: 0, roundNumber: 0, playerName: '', playerPic: '', result: '', time: new Date(), timestamped: new Date,

          team: {
            id: 0, compID: 0, name: '', pic: '', rating: 0, compsCount: 0, club: true, inComp: false, hasFixture: false, timestamped: new Date(),

            comps: [{
              id: 0, roundID: 0, name: '', pic: '', teamsCount: 0, teamsRemaining: 0, teamsTotal: 0, legacy: false, active: false, hasTeam: false, openNotified: false, open: new Date(), timestamped: new Date(),

              admin: {
                id: 0, artID: 0, firstName: '', lastName: '', fullName: '', email: '', password: '', code: '', canAdd: true, notify: false, admin: false, fullAdmin: false,
                art: { id: 0, index: 0, firstName: '', lastName: '', fullName: '', path: '', timestamped: new Date() }
              },

            }]
          }
        }
      }],

    }
  };

}

