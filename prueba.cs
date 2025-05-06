public string Login(User user)
{
    //al iniciar sesion
    var LoginUser = _users.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password);
    if(LoginUser == null)
    {
        return string.Empty;
    } //verifico los datos del usuario; realmente deberia ser con encriptamiento y asi
    //pero a efectos de este taller lo haremos asi, una vez verificados los datos del user
    //con la clase handler del token (Jwt Bearer)
    //hago la soperaciones necesairas para poder crear el propio token
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes("6f4d75aab32aef76b24c058d1bf7b979");
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
    {
        new Claim(ClaimTypes.Name, LoginUser.UserName),
        new Claim("id", LoginUser.Id.ToString())
    }),
        Expires = DateTime.UtcNow.AddMinutes(30),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    string userToken = tokenHandler.WriteToken(token);
    return userToken;
}