import parseJwt from "../AdditionalComponents/parseJwt";

export default async function getUserRole() {
  const token = localStorage.getItem("token");
  if (token != "undefined") {
    const decoded = await parseJwt(token);

    return decoded.role;
  }
}
