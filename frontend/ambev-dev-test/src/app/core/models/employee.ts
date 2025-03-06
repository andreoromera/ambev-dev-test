import Phone from './phone';
import Role from './role';

export default class Employee {
  firstName = '';
  lastName = '';
  email = '';
  document = '';
  birthDate = '';
  superior = '';
  password = '';
  passwordConfirm = '';
  role?: Role;
  phones: Phone[] = [];

  //prettier-ignore
  roleIsValid(loggedUserRoleId: number) {
    return this.role
        && this.role.id > 0
        && this.role.id < loggedUserRoleId;
  }
}
