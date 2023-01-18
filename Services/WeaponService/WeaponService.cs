using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext context;
        public IHttpContextAccessor httpContextAccessor { get; }
        private readonly IMapper mapper;
        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;

        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                        c.User!.Id == int.Parse(httpContextAccessor.HttpContext!.User
                            .FindFirstValue(ClaimTypes.NameIdentifier)!));

                if (character is null)
                {
                    response.Success = false;
                    response.Message = "Character not found.";
                    return response;
                }

                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = character
                };

                context.Weapons.Add(weapon);
                await context.SaveChangesAsync();

                response.Data = mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}