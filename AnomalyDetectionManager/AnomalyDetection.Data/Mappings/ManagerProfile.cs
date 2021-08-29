using AnomalyDetection.Data.Model.Api;
using AnomalyDetection.Data.Model.Db;
using AutoMapper;

namespace AnomalyDetection.Data.Mappings
{
    public class ManagerProfile : Profile
    {
        public ManagerProfile()
        {
            // CreateMap<ApiCrudModel, DbCrudModel>()
            //     .Include<ApiDatasource, DbDatasource>()
            //     .Include<ApiMetric, DbMetric>()
            //     .Include<ApiTrainingJob, DbTrainingJob>();
            // .ReverseMap();

            CreateMap<ApiDatasource, DbDatasource>();
            CreateMap<ApiMetric, DbMetric>()
                .ForMember(dest => dest.DatasourceId, options => options.MapFrom(source => source.Datasource.Id))
                .ForMember(dest => dest.Datasource, options => options.Ignore());
            CreateMap<ApiTrainingJob, DbTrainingJob>();

            // REVERSED DIRECTION

            CreateMap<DbDatasource, ApiDatasource>()
                // .ForPath(dest => dest.Password, options => options.Ignore())
                .ForMember(dest => dest.Password, options => options.Ignore()); // hiding passwords

            // CreateMap<ApiDatasource, ApiDatasource>()
            //     .ForMember(dest => dest.Password, options => options.Ignore()); // hiding passwords

            // CreateMap<DbDatasource, DbDatasource>()
            //     .ForMember(dest => dest.Password, options => options.Ignore()); // hiding passwords

            CreateMap<DbMetric, ApiMetric>();
                // .ForPath(dest => dest.Datasource.Password, options => options.Ignore());
                // .ForMember(dest => dest.Datasource, options => options.Ignore());

                // .ForMember(dest => dest.Datasource, options => options.MapFrom(m => m.Datasource));
                // .ForPath(dest => dest.Datasource.Password, options => options.Ignore());
                //  .ForMember(dest => dest.Datasource, options => options.UseDestinationValue());
                // .ForMember(dest => dest.Datasource.Password, options => options.Ignore()); // hiding passwords
            CreateMap<DbTrainingJob, ApiTrainingJob>();
                // .ForMember(dest => dest.Metric.Datasource.Password, options => options.Ignore()); // hiding passwords

        }
    }
}