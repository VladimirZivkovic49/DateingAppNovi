export interface User
{
username: string;
token: string ;
photoUrl:string ;
//(L159)
knownAs:string ;
gender:string ;
//(159)
//(L212)
roles: string[] | null;
//(L212)
}