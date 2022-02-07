using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SaikiAPI.EntityModels;
using SaikiAPI.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SaikiAPI.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly JwtSetting _jwtSetting;


        public EmployeeService(UserManager<ApplicationUser> userManager, IOptions<JwtSetting> options, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _jwtSetting = options.Value;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<ApplicationUser> AuthenticateUser(UserCred userCred)
        {
            var user = await _userManager.FindByEmailAsync(userCred.UserName);

            if (user != null && await _userManager.CheckPasswordAsync(user, userCred.Password))
            {
                return new ApplicationUser()
                {
                    UserName = userCred.UserName,
                    Id  = user.Id,
                    BearerToken = GenerateAccessToken(user.Id)
                };
            }
            else
            {
                return new ApplicationUser();
            }
        }

        public async Task<ApplicationUser> GetUserData(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                return user;
            }
            else
            {
                throw new Exception("User does not exists");
            }
        }

        private MemoryStream SerializeToStream(ApplicationUser applicationUser)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ApplicationUser));
            MemoryStream stream = new MemoryStream() { Position = 0};
            try
            {
                serializer.Serialize(stream, applicationUser);
            }
            catch (SerializationException ex)
            {
                throw new ApplicationException("The object graph could not be serialized", ex);
            }
            // Return the streamed object graph.
            return stream;
        }

        public async Task<bool> UploadDataToAzure(string userId)
        { 
            var user = await _userManager.FindByIdAsync(userId);

            BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetSection("Blob").GetValue<string>("ConnectionString"));
            

            var containerClient = blobServiceClient.GetBlobContainerClient("user-data");
            containerClient.CreateIfNotExists();
            //Delete if the BLOB is already eisting for the same user_id
            var bolbClient = containerClient.GetBlobClient(String.Format("{0}.xml", userId)).DeleteIfExists();
           
            var streamedData = SerializeToStream(user);
            streamedData.Position = 0;
            var result = containerClient.UploadBlob(String.Format("{0}.xml", userId), streamedData, cancellationToken: System.Threading.CancellationToken.None);
            if (result.GetRawResponse().Status == 201 && string.Equals(result.GetRawResponse().ReasonPhrase, "Created", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public string DownloadDataFromAzure(string userId)
        {
            var blobName = String.Format("{0}.xml", userId);

            var folderPath = "Users\\Data\\Download\\";

            var serverPath = Path.Combine(_webHostEnvironment.ContentRootPath, folderPath);

            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            var finalPath = string.Concat(serverPath, blobName);

            if (!File.Exists(finalPath))
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_configuration.GetSection("Blob").GetValue<string>("ConnectionString"));

                var containerClient = blobServiceClient.GetBlobContainerClient("user-data");

                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                var result = blobClient.DownloadTo(finalPath);

                if (result.Status == 206)
                {
                    return finalPath;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception($"File Exists @ {finalPath}");
            }

        }

        private string GenerateAccessToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, Convert.ToString(userId))
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
